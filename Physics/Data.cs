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
        private List<Vector3> forceDrag;
        private List<Vector3> forceMagnus;
        private List<Vector3> forceNet;
        private List<Vector2> k1;
        private List<Vector2> k2;
        private List<Vector2> k3;
        private List<Vector2> k4;
        private List<Vector2> k;

        #endregion

        #region Constructor

        public Data(string name)
        {
            this.name = name;
            time = new List<double>();
            positions = new List<Vector3>();
            velocites = new List<Vector3>();
            forceGravity = new List<Vector3>();
            forceDrag = new List<Vector3>();
            forceMagnus = new List<Vector3>();
            forceNet = new List<Vector3>();

            k1 = new List<Vector2>();
            k2 = new List<Vector2>();
            k3 = new List<Vector2>();
            k4 = new List<Vector2>();
            k = new List<Vector2>();
        }

        #endregion

        public void AddForces(Vector3 fG, Vector3 fD, Vector3 fM, Vector3 fN)
        {
            forceGravity.Add(fG);
            forceDrag.Add(fD);
            forceMagnus.Add(fM);
            forceNet.Add(fN);
        }

        public void AddRK4(Vector2 pv0, Vector2 k1, Vector2 k2, Vector2 k3, Vector2 k4, Vector2 k, double time)
        {
            positions.Add(pv0.X);
            velocites.Add(pv0.Y);
            this.k1.Add(k1);
            this.k2.Add(k2);
            this.k3.Add(k3);
            this.k4.Add(k4);
            this.k.Add(k);
            this.time.Add(time);
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

                int temp = 5;
                for (int i = 1; i <= 4; i++)
                {
                    ws.Cells["A" + temp].Value = "K" + i;
                    ws.Cells["A" + (temp + 1)].Value = "Force Gravity";
                    ws.Cells["A" + (temp + 2)].Value = "Force Drag";
                    ws.Cells["A" + (temp + 3)].Value = "Force Magnus";
                    ws.Cells["A" + (temp + 4)].Value = "Force Net";
                    temp += 6;
                }

                ws.Cells["A" + temp].Value = "K";

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

                    temp = 5;
                    List<List<Vector2>> klists = new List<List<Vector2>>(){ k1, k2, k3, k4 };
                    for (int j = 0; j < 4; j++)
                    {
                        ws.Cells[col + temp].Value = "Velocity: " + klists[j][i].X +
                            "\nAcceleration: " + klists[j][i].Y;
                        ws.Cells[col + (temp + 1)].Value = forceGravity[j + (4 * i)].ToString();
                        ws.Cells[col + (temp + 2)].Value = forceDrag[j + (4 * i)].ToString();
                        ws.Cells[col + (temp + 3)].Value = forceMagnus[j + (4 * i)].ToString();
                        ws.Cells[col + (temp + 4)].Value = forceNet[j + (4 * i)].ToString();
                        temp += 6;
                    }

                    ws.Cells[col + temp].Value = k[i];
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
