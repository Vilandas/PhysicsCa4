﻿using GDLibrary.Actors;
using GDLibrary.Enums;
using GDLibrary.Events;
using GDLibrary.GameComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GDLibrary.Managers
{
    /// <summary>
    /// Stores and calls an Update on all drawn 3D objects. This is a pausable component (i.e. listens for menu play/pause events)
    /// and so it extends PausableDrawableGameComponent. This class also subscribes to, and adds events to handle, relevent events
    /// (e.g. object remove, Object add, object change transparency)
    /// </summary>
    /// <see cref="GDLibrary.GameComponents.PausableDrawableGameComponent.SubscribeToEvents"/>
    public class ObjectManager : PausableGameComponent
    {
        #region Fields

        private List<DrawnActor3D> opaqueList, transparentList;
        private int idCount;

        public IEnumerable<DrawnActor3D> OpaqueList
        {
            get
            {
                return opaqueList;
            }
        }
        public IEnumerable<DrawnActor3D> TransparentList
        {
            get
            {
                return transparentList;
            }
        }

        #endregion Fields

        #region Constructors & Core

        public ObjectManager(Game game, StatusType statusType,
          int initialOpaqueDrawSize, int initialTransparentDrawSize) : base(game, statusType)
        {
            opaqueList = new List<DrawnActor3D>(initialOpaqueDrawSize);
            transparentList = new List<DrawnActor3D>(initialTransparentDrawSize);
            idCount = 0;
        }

        #region Handle Events
        protected override void SubscribeToEvents()
        {
            //remove
            EventDispatcher.Subscribe(EventCategoryType.Object, HandleEvent);

            //add more ObjectManager specfic subscriptions here...
            EventDispatcher.Subscribe(EventCategoryType.Player, HandleEvent);

            //call base method to subscribe to menu event
            base.SubscribeToEvents();
        }

        protected override void HandleEvent(EventData eventData)
        {
            //if this event relates to adding, removing, changing an object
            if (eventData.EventCategoryType == EventCategoryType.Object)
            {
                HandleObjectCategoryEvent(eventData);
            }
            else if (eventData.EventCategoryType == EventCategoryType.Player)
            {
                HandlePlayerCategoryEvent(eventData);
            }

            //pass event to base (in case it is a menu event)
            base.HandleEvent(eventData);
        }

        private void HandlePlayerCategoryEvent(EventData eventData)
        {
            if (eventData.EventActionType == EventActionType.OnWin)
            {
                //gets params and add win animation
            }
        }

        private void HandleObjectCategoryEvent(EventData eventData)
        {
            if (eventData.EventActionType == EventActionType.OnRemoveActor)
            {
                DrawnActor3D removeObject = eventData.Parameters[0] as DrawnActor3D;
                opaqueList.Remove(removeObject);
            }
            else if (eventData.EventActionType == EventActionType.OnAddActor)
            {
                ModelObject modelObject = eventData.Parameters[0] as ModelObject;
                if (modelObject != null)
                {
                    Add(modelObject);
                }
            }

            //example predicate
            //RemoveFirstIf((drawnActor3D) =>
            //(drawnActor3D.ActorType == ActorType.Pickup && drawnActor3D.EffectParameters.Alpha == 1));
        }
        #endregion Handle Events

        /// <summary>
        /// Add the actor to the appropriate list based on actor transparency
        /// </summary>
        /// <param name="actor"></param>
        public void Add(DrawnActor3D actor)
        {
            idCount++;
            if (actor.EffectParameters.Alpha < 1)
                transparentList.Add(actor);
            else
                opaqueList.Add(actor);
        }

        /// <summary>
        /// Remove the first instance of an actor corresponding to the predicate
        /// </summary>
        /// <param name="predicate">Lambda function which allows ObjectManager to uniquely identify an actor</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool RemoveFirstIf(Predicate<DrawnActor3D> predicate)
        {
            //to do...improve efficiency by adding DrawType enum
            int position = -1;
            bool wasRemoved = false;

            position = opaqueList.FindIndex(predicate);   //N
            if (position != -1)
            {
                opaqueList.RemoveAt(position);
                wasRemoved = true;
            }

            position = transparentList.FindIndex(predicate);  //M
            if (position != -1)
            {
                transparentList.RemoveAt(position);
                wasRemoved = true;
            }

            //O(N + M)
            return wasRemoved;
        }

        /// <summary>
        /// Remove all occurences of any actors corresponding to the predicate
        /// </summary>
        /// <param name="predicate">Lambda function which allows ObjectManager to uniquely identify one or more actors</param>
        /// <returns>Count of the number of removed actors</returns>
        public int RemoveAll(Predicate<DrawnActor3D> predicate)
        {
            //to do...improve efficiency by adding DrawType enum
            int count = 0;
            count = opaqueList.RemoveAll(predicate);
            count += transparentList.RemoveAll(predicate);
            return count;
        }

        /// <summary>
        /// Called to update the lists of actors
        /// </summary>
        /// <see cref="PausableDrawableGameComponent.Update(GameTime)"/>
        /// <param name="gameTime">GameTime object</param>
        protected override void ApplyUpdate(GameTime gameTime)
        {
            foreach (DrawnActor3D actor in opaqueList)
            {
                if ((actor.StatusType & StatusType.Update) == StatusType.Update)
                    actor.Update(gameTime);
            }

            foreach (DrawnActor3D actor in transparentList)
            {
                if ((actor.StatusType & StatusType.Update) == StatusType.Update)
                    actor.Update(gameTime);
            }
        }

        #endregion Constructors & Core

        #region Custom (Not Nialls)

        public bool RemoveByID(string id)
        {
            for (int i = 0; i < opaqueList.Count; i++)
            {
                if (opaqueList[i].ID.Equals(id))
                {
                    opaqueList.RemoveAt(i);
                    return true;
                }
            }

            for (int i = 0; i < transparentList.Count; i++)
            {
                if (opaqueList[i].ID.Equals(id))
                {
                    transparentList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public List<DrawnActor3D> GetActorList(ActorType actorType)
        {
            List<DrawnActor3D> list = new List<DrawnActor3D>();
            foreach (DrawnActor3D actor in opaqueList)
            {
                if (actor.ActorType == actorType)
                {
                    list.Add(actor);
                }
            }
            return list;
        }

        public int ListSize()
        {
            return opaqueList.Count + transparentList.Count;
        }

        public int NewID()
        {
            return idCount;
        }

        #endregion
    }
}