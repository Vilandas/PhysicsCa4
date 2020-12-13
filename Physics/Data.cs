using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OfficeOpenXml;

namespace Physics
{
    public class Data
    {
        #region Fields

        private string name;
        private List<double> time;
        private List<Vector3> positions;
        private List<Vector3> velocites;
        private List<Vector3> forceGravity;
        private List<Vector3> forceGravityNormal;
        private List<Vector3> forceGravityPlane;
        private List<Vector3> ForceNormal;
        private List<double> forceFrictionMax;

        private List<double> forceFrictionLength;
        private List<Vector3> forceFrictionHat;
        private List<Vector3> forceFriction;
        private List<Vector3> forceNet;
        private List<Vector3> acceleration;

        private List<Vector2> k1;
        private List<Vector2> k2;
        private List<Vector2> k3;
        private List<Vector2> k4;
        private List<Vector2> k;
        private bool kinetic;

        #endregion

        #region Constructor

        public Data(string name)
        {
            this.name = name;
            time = new List<double>();
            positions = new List<Vector3>();
            velocites = new List<Vector3>();
            forceGravity = new List<Vector3>();
            forceGravityNormal = new List<Vector3>();
            forceGravityPlane = new List<Vector3>();
            ForceNormal = new List<Vector3>();
            forceFrictionMax = new List<double>();

            forceFrictionLength = new List<double>();
            forceFrictionHat = new List<Vector3>();
            forceFriction = new List<Vector3>();
            forceNet = new List<Vector3>();
            acceleration = new List<Vector3>();
            k1 = new List<Vector2>();
            k2 = new List<Vector2>();
            k3 = new List<Vector2>();
            k4 = new List<Vector2>();
            k = new List<Vector2>();
            kinetic = false;
        }

        #endregion

        public void AddForces(Vector3 fG, Vector3 fGN, Vector3 fGP, Vector3 fN,
            double ffMax)
        {
            forceGravity.Add(fG);
            forceGravityNormal.Add(fGN);
            forceGravityPlane.Add(fGP);
            ForceNormal.Add(fN);
            forceFrictionMax.Add(ffMax);
        }

        public void AddKinetic(double fFLength, Vector3 fFHat, Vector3 fF,
            Vector3 fN, Vector3 acceleration)
        {
            kinetic = true;
            forceFrictionLength.Add(fFLength);
            forceFrictionHat.Add(fFHat);
            forceFriction.Add(fF);
            forceNet.Add(fN);
            this.acceleration.Add(acceleration);
        }

        public void AddPV(Vector2 pv0, double time)
        {
            positions.Add(pv0.X);
            velocites.Add(pv0.Y);
            this.time.Add(time);
        }

        public void AddRK4(Vector2 k1, Vector2 k2, Vector2 k3, Vector2 k4, Vector2 k)
        {
            this.k1.Add(k1);
            this.k2.Add(k2);
            this.k3.Add(k3);
            this.k4.Add(k4);
            this.k.Add(k);
        }

        public void ExportData()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add(name);
                ws.DefaultColWidth = 78;
                ws.Cells.Style.WrapText = true;
                ws.Column(1).Width = 20;

                ws.Cells["A1"].Value = "Time";
                ws.Cells["A2"].Value = "Position";
                ws.Cells["A3"].Value = "Velocity";

                ws.Cells["A4"].Value = "Force Gravity";
                ws.Cells["A5"].Value = "Force Gravity Plane";
                ws.Cells["A6"].Value = "Force Gravity Normal";
                ws.Cells["A7"].Value = "Force Friction Max";
                ws.Cells["A8"].Value = "||F̅gp||";

                ws.Cells["A9"].Value = "Force Friction Length";
                ws.Cells["A10"].Value = "Force Friction Hat";
                ws.Cells["A11"].Value = "Force Friction";
                ws.Cells["A12"].Value = "Force Net";
                ws.Cells["A13"].Value = "Acceleration";

                ws.Cells["A15"].Value = "K1";
                ws.Cells["A16"].Value = "K2";
                ws.Cells["A17"].Value = "K3";
                ws.Cells["A18"].Value = "K4";
                ws.Cells["A19"].Value = "K";

                string col = "A";
                for (int i = 0; i < time.Count; i++)
                {
                    char[] array = col.ToCharArray();
                    array[array.Length - 1]++;
                    for (int j = array.Length - 1; j >= 0; j--)
                    {
                        if (array[j] > 'Z')
                        {
                            array[j] = 'A';
                            if (j != 0)
                            {
                                array[j - 1]++;
                            }
                            else col = 'A' + new string(array);
                        }
                        else if (j == 0)
                            col = new string(array);
                    }

                    ws.Cells[col + 1].Value = time[i];
                    ws.Cells[col + 2].Value = positions[i].ToString();
                    ws.Cells[col + 3].Value = velocites[i].ToString();

                    ws.Cells[col + 4].Value = forceGravity[i].ToString();
                    ws.Cells[col + 5].Value = forceGravityPlane[i].ToString();
                    ws.Cells[col + 6].Value = forceGravityNormal[i].ToString();
                    ws.Cells[col + 7].Value = forceFrictionMax[i].ToString();
                    ws.Cells[col + 8].Value = forceGravityPlane[i].Length.ToString();

                    if (kinetic)
                    {
                        ws.Cells[col + 9].Value = forceFrictionLength[i].ToString();
                        ws.Cells[col + 10].Value = forceFrictionHat[i].ToString();
                        ws.Cells[col + 11].Value = forceFriction[i].ToString();
                        ws.Cells[col + 12].Value = forceNet[i].ToString();
                        ws.Cells[col + 13].Value = acceleration[i].ToString();
                        ws.Cells[col + 15].Value = k1[i].ToString();
                        ws.Cells[col + 16].Value = k2[i].ToString();
                        ws.Cells[col + 17].Value = k3[i].ToString();
                        ws.Cells[col + 18].Value = k4[i].ToString();
                        ws.Cells[col + 19].Value = k[i].ToString();
                    }
                }
                try
                {
                    p.SaveAs(new FileInfo(name + ".xlsx"));
                }
                catch(Exception e)
                {
                    Debug.WriteLine("ERROR - Could not save file");
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
