using GDLibrary.Actors;
using GDLibrary.Debug;
using GDLibrary.Enums;
using GDLibrary.Events;
using GDLibrary.Factories;
using GDLibrary.Interfaces;
using GDLibrary.Managers;
using GDLibrary.Parameters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GDGame
{
    public class Main : Game
    {
        #region Fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //effects used by primitive objects (wireframe, lit, unlit) and model objects
        private BasicEffect unlitTexturedEffect, unlitWireframeEffect, modelEffect;

        //managers in the game
        private CameraManager<Camera3D> cameraManager;
        private ObjectManager objectManager;
        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private UIController uiController;
        

        //defines centre point for the mouse i.e. (w/2, h/2)
        private Vector2 screenCentre;

        //size of the skybox and ground plane
        private float worldScale = 1000;

        private VertexPositionColorTexture[] vertices;
        private Texture2D backSky, leftSky, rightSky, frontSky, topSky, grass, crate;

        //font used to show debug info
        private SpriteFont debugFont;

        private PrimitiveObject archetypalTexturedQuad;


        private PrimitiveObject primitiveObject = null;
        private Model cube;
        private EventDispatcher eventDispatcher;
        private Viewport halfSizeViewport;
        private RenderManager renderManager;

        private ModelObject box;

        #endregion Fields

        #region Constructors

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        #endregion Constructors

        #region Initialization - Managers, Cameras, Effects, Textures

        protected override void Initialize()
        {
            //set game title
            Window.Title = "Physics Simulation";

            //note that we moved this from LoadContent to allow InitDebug to be called in Initialize
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            InitEventDispatcher();
            //managers
            InitManagers();

            //resources and effects
            InitVertices();
            InitTextures();
            InitModels();
            InitFonts();
            InitEffects();

            //drawn content
            InitDrawnContent();

            //cameras - notice we moved the camera creation BELOW where we created the drawn content - see DriveController
            InitCameras3D();

            InitBox();

            //graphic settings - see https://en.wikipedia.org/wiki/Display_resolution#/media/File:Vector_Video_Standards8.svg
            InitGraphics(1440, 1050);

            //debug info
            //InitDebug();

            uiController = new UIController(this, objectManager, keyboardManager, box, cameraManager[0], archetypalTexturedQuad);
            Components.Add(uiController);

            base.Initialize();
        }

        private void InitEventDispatcher()
        {
            eventDispatcher = new EventDispatcher(this);
            Components.Add(eventDispatcher);
        }

        private void HandleEvent(EventData eventData)
        {
        }

        private void InitManagers()
        {
            //camera
            cameraManager = new CameraManager<Camera3D>(this, StatusType.Update);
            Components.Add(cameraManager);

            //keyboard
            keyboardManager = new KeyboardManager(this);
            Components.Add(keyboardManager);

            //mouse
            mouseManager = new MouseManager(this, IsMouseVisible);
            Components.Add(mouseManager);

            //object
            objectManager = new ObjectManager(this, StatusType.Update, 6, 10);

            //render
            renderManager = new RenderManager(this, StatusType.Drawn, ScreenLayoutType.Single,
                objectManager, cameraManager);

            Components.Add(renderManager);

            Components.Add(objectManager);
        }

        private void InitDebug()
        {
            //create the debug drawer to draw debug info
            DebugDrawer debugDrawer = new DebugDrawer(this, _spriteBatch, debugFont,
                cameraManager, objectManager);

            //set the debug drawer to be drawn AFTER the object manager to the screen
            debugDrawer.DrawOrder = 2;

            //add the debug drawer to the component list so that it will have its Update/Draw methods called each cycle.
            Components.Add(debugDrawer);
        }

        private void InitFonts()
        {
            debugFont = Content.Load<SpriteFont>("Assets/Fonts/debug");
        }

        private void InitCameras3D()
        {
            Transform3D transform3D = null;
            Camera3D camera3D = null;
            Viewport viewPort = new Viewport(0, 0, 1440, 1050);

            #region Camera - Ball Third Person

            transform3D = new Transform3D(new Vector3(10, 10, 20),
                new Vector3(-1, 0, 0), Vector3.UnitY);

            camera3D = new Camera3D("3rd person",
                ActorType.Camera3D, StatusType.Update, transform3D,
                ProjectionParameters.StandardDeepSixteenTen, viewPort);

            cameraManager.Add(camera3D);

            #endregion

            cameraManager.ActiveCameraIndex = 0; //0, 1, 2, 3
        }

        private void InitEffects()
        {
            //to do...
            unlitTexturedEffect = new BasicEffect(_graphics.GraphicsDevice);
            unlitTexturedEffect.VertexColorEnabled = true; //otherwise we wont see RGB
            unlitTexturedEffect.TextureEnabled = true;

            //wireframe primitives with no lighting and no texture
            unlitWireframeEffect = new BasicEffect(_graphics.GraphicsDevice);
            unlitWireframeEffect.VertexColorEnabled = true;

            //model effect
            //add a ModelObject
            modelEffect = new BasicEffect(_graphics.GraphicsDevice);
            modelEffect.TextureEnabled = true;
            modelEffect.LightingEnabled = true;
            modelEffect.PreferPerPixelLighting = true;
            //   this.modelEffect.SpecularPower = 512;
            //  this.modelEffect.SpecularColor = Color.Red.ToVector3();
            modelEffect.EnableDefaultLighting();
        }

        private void InitTextures()
        {
            //step 1 - texture
            backSky
                = Content.Load<Texture2D>("Assets/Textures/Skybox/back");
            leftSky
               = Content.Load<Texture2D>("Assets/Textures/Skybox/left");
            rightSky
              = Content.Load<Texture2D>("Assets/Textures/Skybox/right");
            frontSky
              = Content.Load<Texture2D>("Assets/Textures/Skybox/front");
            topSky
              = Content.Load<Texture2D>("Assets/Textures/Skybox/sky");

            grass
              = Content.Load<Texture2D>("Assets/Textures/Foliage/grass1");

            crate
                = Content.Load<Texture2D>("Assets/Textures/Props/crate1");
        }

        private void InitModels()
        {
            cube 
                = Content.Load<Model>("Assets/Models/cube");
        }

        #endregion Initialization - Managers, Cameras, Effects, Textures

        #region Initialization - Vertices, Archetypes, Helpers, Drawn Content(e.g. Skybox)

        private void InitDrawnContent() //formerly InitPrimitives
        {
            //add archetypes that can be cloned
            InitPrimitiveArchetypes();

            //adds origin helper etc
            InitHelpers();

            //add skybox
            InitSkybox();

            //add grass plane
            InitGround();
        }

        private void InitBox()
        {
            ////////Box
            //transform
            Transform3D transform3D = new Transform3D(new Vector3(0, 100, 0),
                                new Vector3(0, 0, 0),       //rotation
                                new Vector3(1, 1, 1),        //scale
                                    -Vector3.UnitY,         //look
                                    Vector3.UnitZ);         //up

            //effectparameters
            EffectParameters effectParameters = new EffectParameters(modelEffect,
                crate,
                Color.White, 1);

            ModelObject modelObject = new ModelObject("Box", ActorType.Primitive,
                StatusType.Drawn | StatusType.Update, transform3D,
                effectParameters, cube);

            box = modelObject;

            objectManager.Add(modelObject);
        }

        private void InitVertices()
        {
            vertices
                = new VertexPositionColorTexture[4];

            float halfLength = 0.5f;
            //TL
            vertices[0] = new VertexPositionColorTexture(
                new Vector3(-halfLength, halfLength, 0),
                new Color(255, 255, 255, 255), new Vector2(0, 0));

            //BL
            vertices[1] = new VertexPositionColorTexture(
                new Vector3(-halfLength, -halfLength, 0),
                Color.White, new Vector2(0, 1));

            //TR
            vertices[2] = new VertexPositionColorTexture(
                new Vector3(halfLength, halfLength, 0),
                Color.White, new Vector2(1, 0));

            //BR
            vertices[3] = new VertexPositionColorTexture(
                new Vector3(halfLength, -halfLength, 0),
                Color.White, new Vector2(1, 1));
        }

        private void InitPrimitiveArchetypes() //formerly InitTexturedQuad
        {
            Transform3D transform3D = new Transform3D(Vector3.Zero, Vector3.Zero,
               Vector3.One, Vector3.UnitZ, Vector3.UnitY);

            EffectParameters effectParameters = new EffectParameters(unlitTexturedEffect,
                grass, /*bug*/ Color.White, 1);

            IVertexData vertexData = new VertexData<VertexPositionColorTexture>(
                vertices, PrimitiveType.TriangleStrip, 2);

            archetypalTexturedQuad = new PrimitiveObject("original texture quad",
                ActorType.Decorator,
                StatusType.Update | StatusType.Drawn,
                transform3D, effectParameters, vertexData);
        }

        //VertexPositionColorTexture - 4 bytes x 3 (x,y,z) + 4 bytes x 3 (r,g,b) + 4bytes x 2 = 26 bytes
        //VertexPositionColor -  4 bytes x 3 (x,y,z) + 4 bytes x 3 (r,g,b) = 24 bytes
        private void InitHelpers()
        {
            //to do...add wireframe origin
            PrimitiveType primitiveType;
            int primitiveCount;

            //step 1 - vertices
            VertexPositionColor[] vertices = VertexFactory.GetVerticesPositionColorOriginHelper(
                                    out primitiveType, out primitiveCount);

            //step 2 - make vertex data that provides Draw()
            IVertexData vertexData = new VertexData<VertexPositionColor>(vertices,
                                    primitiveType, primitiveCount);

            //step 3 - make the primitive object
            Transform3D transform3D = new Transform3D(new Vector3(0, 20, 0),
                Vector3.Zero, new Vector3(10, 10, 10),
                Vector3.UnitZ, Vector3.UnitY);

            EffectParameters effectParameters = new EffectParameters(unlitWireframeEffect,
                null, Color.White, 1);

            //at this point, we're ready!
            PrimitiveObject primitiveObject = new PrimitiveObject("origin helper",
                ActorType.Helper, StatusType.Drawn, transform3D, effectParameters, vertexData);

            objectManager.Add(primitiveObject);
        }

        private void InitSkybox()
        {
            //back
            primitiveObject = archetypalTexturedQuad.Clone() as PrimitiveObject;
            //  primitiveObject.StatusType = StatusType.Off; //Experiment of the effect of StatusType
            primitiveObject.ID = "sky back";
            primitiveObject.EffectParameters.Texture = backSky;
            primitiveObject.Transform3D.Scale = new Vector3(worldScale, worldScale, 1);
            primitiveObject.Transform3D.RotationInDegrees = new Vector3(0, 180, 0);
            primitiveObject.Transform3D.Translation = new Vector3(0, 0, -worldScale / 2.0f);
            objectManager.Add(primitiveObject);

            //left
            primitiveObject = archetypalTexturedQuad.Clone() as PrimitiveObject;
            primitiveObject.ID = "left back";
            primitiveObject.EffectParameters.Texture = leftSky;
            primitiveObject.Transform3D.Scale = new Vector3(worldScale, worldScale, 1);
            primitiveObject.Transform3D.RotationInDegrees = new Vector3(0, 90, 0);
            primitiveObject.Transform3D.Translation = new Vector3(worldScale / 2.0f, 0, 0);
            objectManager.Add(primitiveObject);

            //right
            primitiveObject = archetypalTexturedQuad.Clone() as PrimitiveObject;
            primitiveObject.ID = "sky right";
            primitiveObject.EffectParameters.Texture = rightSky;
            primitiveObject.Transform3D.Scale = new Vector3(worldScale, worldScale, 20);
            primitiveObject.Transform3D.RotationInDegrees = new Vector3(0, -90, 0);
            primitiveObject.Transform3D.Translation = new Vector3(-worldScale / 2.0f, 0, 0);
            objectManager.Add(primitiveObject);

            //top
            primitiveObject = archetypalTexturedQuad.Clone() as PrimitiveObject;
            primitiveObject.ID = "sky top";
            primitiveObject.EffectParameters.Texture = topSky;
            primitiveObject.Transform3D.Scale = new Vector3(worldScale, worldScale, 1);
            primitiveObject.Transform3D.RotationInDegrees = new Vector3(-90, -90, 0);
            primitiveObject.Transform3D.Translation = new Vector3(0, worldScale / 2.0f, 0);
            objectManager.Add(primitiveObject);

            //front
            primitiveObject = archetypalTexturedQuad.Clone() as PrimitiveObject;
            primitiveObject.ID = "sky front";
            primitiveObject.EffectParameters.Texture = frontSky;
            primitiveObject.Transform3D.Scale = new Vector3(worldScale, worldScale, 1);
            primitiveObject.Transform3D.Translation = new Vector3(0, 0, worldScale / 2.0f);
            objectManager.Add(primitiveObject);
        }

        private void InitGround()
        {
            //grass
            primitiveObject = archetypalTexturedQuad.Clone() as PrimitiveObject;
            primitiveObject.ID = "grass";
            primitiveObject.EffectParameters.Texture = grass;
            primitiveObject.Transform3D.Scale = new Vector3(worldScale, worldScale, 1);
            primitiveObject.Transform3D.RotationInDegrees = new Vector3(90, 90, 0);
            objectManager.Add(primitiveObject);
        }

        private void InitGraphics(int width, int height)
        {
            //set resolution
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;

            //dont forget to apply resolution changes otherwise we wont see the new WxH
            _graphics.ApplyChanges();

            //set screen centre based on resolution
            screenCentre = new Vector2(width / 2, height / 2);

            //set cull mode to show front and back faces - inefficient but we will change later
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            _graphics.GraphicsDevice.RasterizerState = rs;

            //we use a sampler state to set the texture address mode to solve the aliasing problem between skybox planes
            SamplerState samplerState = new SamplerState();
            samplerState.AddressU = TextureAddressMode.Clamp;
            samplerState.AddressV = TextureAddressMode.Clamp;
            _graphics.GraphicsDevice.SamplerStates[0] = samplerState;

            //set blending
            _graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            //set screen centre for use when centering mouse
            screenCentre = new Vector2(width / 2, height / 2);
        }

        protected override void LoadContent()
        {
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion Initialization - Vertices, Archetypes, Helpers, Drawn Content(e.g. Skybox)

        #region Update & Draw

        protected override void Update(GameTime gameTime)
        {
            if (keyboardManager.IsFirstKeyPress(Keys.Escape))
            {
                Exit();
            }

            if (keyboardManager.IsFirstKeyPress(Keys.C))
            {
                cameraManager.CycleActiveCamera();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            _graphics.GraphicsDevice.DepthStencilState = dss;

            _spriteBatch.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            base.Draw(gameTime);
        }

        #endregion Update & Draw
    }
}