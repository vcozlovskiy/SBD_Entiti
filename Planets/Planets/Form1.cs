using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Data.SqlClient;

namespace Planets
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Planet[] temp1;
        List<Planet> PlanetsList = new List<Planet>();
        private void Form1_Load(object sender, EventArgs e)
        {
            Planet Mercyriy = new Planet
            {
                Id = 1,
                Name = "Mercyriy",
                Radius = 24397000,
                Mass = 3.33 * Math.Pow(10.0, 23.0)
            };
            Planet Venera = new Planet
            {
                Id = 2,
                Name = "Venera",
                Radius = 6051000,
                Mass = 4.86 * Math.Pow(10.0, 24)
            };
            Planet Ertch = new Planet
            {
                Id = 3,
                Name = "Ertch",
                Radius = 6371000,
                Mass = 5.972 * Math.Pow(10.0, 24)
            };
            Planet Mars = new Planet
            {
                Id = 4,
                Name = "Mars",
                Radius = 3396000,
                Mass = 6.417 * Math.Pow(10.0, 23.0)
            };
            Planet Ypiter = new Planet
            {
                Id = 5,
                Name = "Ypiter",
                Radius = 69911000,
                Mass = 1.89 * Math.Pow(10, 27)
            };
            Planet Saturn = new Planet
            {
                Id = 6,
                Name = "Saturn",
                Radius = 58388000,
                Mass = 5.68 * Math.Pow(10, 26)
            };
            Planet Uran = new Planet
            {
                Id = 7,
                Name = "Uran",
                Radius = 25559000,
                Mass = 8.681 * Math.Pow(10, 26)
            };
            Planet Neptun = new Planet
            {
                Id = 8,
                Name = "Neptun",
                Radius = 24662000,
                Mass = 1.024 * Math.Pow(10, 26)
            };
            using (PlanetContext PC = new PlanetContext())
            {
                
                temp1 = new Planet[] { Mercyriy, Venera, Ertch, Mars, Ypiter, Saturn, Uran, Neptun };
                if (PC.Planets.ToList().Count < 8) 
                {
                    PC.Planets.AddRange(temp1);
                    PC.SaveChanges();
                }
                PlanetsList = new List<Planet>();
                List<Planet> PlanetsListNow = PC.Planets.ToList();
                for (int i = 0; PC.Planets.ToList().Count() > PlanetsList.Count(); i++)
                {
                    PlanetsList.Add(PlanetsListNow[i]);
                }
            }
            foreach (Planet p in PlanetsList)
            {
                comboBox1.Items.Add(p.Name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (PlanetContext P = new PlanetContext())
            {
                Planet WhatVel = PlanetsList[comboBox1.SelectedIndex];
                out_speed.Text = WhatVel.CalcFirtCosmVel().ToString("F2") + ", М/С";
                planet_name.Text = WhatVel.Name;
                planet_mass.Text = WhatVel.Mass.ToString();
                planet_radius.Text = WhatVel.Radius.ToString();
                textBox1.Text = WhatVel.CalcSecndCosmVel().ToString("F2") + ", М/С";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (PlanetContext P = new PlanetContext())
            {
                double.TryParse(planet_mass.Text,out double mass);
                double.TryParse(planet_radius.Text, out double rad);
                Planet PlanetNew = new Planet()
                {
                    Id = (P.Planets.Count() + 1),
                    Name = planet_name.Text,
                    Mass = mass,
                    Radius = rad
                };
                P.Planets.Add(PlanetNew);
                if (checkBox1.Checked == true)
                {
                    P.SaveChanges();
                    comboBox1.Items.Add(PlanetNew.Name);
                    PlanetsList.Add(PlanetNew);
                }
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            string connectionString = @"data source=DESKTOP-63OB1FE\MSSWL;Initial Catalog=PAL;Integrated Security=True;";
            string sqlExpression = $"DELETE FROM Planets WHERE Name = '{comboBox1.SelectedItem.ToString()}'";
            using (SqlConnection connection =new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sqlExpression, connection);
                cmd.ExecuteNonQuery();
                comboBox1.SelectedIndex -= 1;
            }
            using (PlanetContext PC = new PlanetContext())
            {
                PlanetsList = new List<Planet>();
                List<Planet> PlanetsListNow = PC.Planets.ToList();
                comboBox1.Items.Clear();
                for (int i = 0; PC.Planets.ToList().Count() - 1 > PlanetsList.Count(); i++)
                {
                    PlanetsList.Add(PlanetsListNow[i]);
                    comboBox1.Items.Add(PlanetsListNow[i].Name);
                }
            }

        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            using (PlanetContext P = new PlanetContext())
            {
                Planet p = P.Planets.ToList()[comboBox1.SelectedIndex];
                planet_name.Text = p.Name;
                planet_mass.Text = p.Mass.ToString();
                planet_radius.Text = p.Radius.ToString();
                if (planet_name.Text != null && planet_mass.Text != null && planet_radius.Text != null)
                {
                    p.Name = planet_name.Text;
                    double m;
                    double.TryParse(planet_mass.Text, out m);
                    p.Mass = m;
                    double.TryParse(planet_radius.Text, out m);
                    p.Radius = m;
                }
                P.SaveChanges();


            }
        }
    }
    class PlanetContext : DbContext
    {
        public PlanetContext()
            : base("DbConnection")
        { }
        public DbSet<Planet> Planets { get; set; }
    }
    public class Planet
    {
        private static double _GravConst = 6.67 * Math.Pow(10, -11);
        public int Id { get; set; }
        public string Name { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }
        public double CalcFirtCosmVel()
        {
            return Math.Sqrt(6.67 * Math.Pow(10, -11) * this.Mass / this.Radius);
        }
        public double CalcSecndCosmVel()
        {
            return Math.Sqrt(2 * 6.67 * Math.Pow(10, -11) * this.Mass / this.Radius);
        }
    }
}
