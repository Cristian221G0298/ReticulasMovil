using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReticulasMovil.Model
{
    public enum Carreras { Sistemas, Mecatronica, Industrial }
    public class Materia
    {
        public string Nombre { get; set; } = null;
        public string Clave { get; set; } = null;
        public int Semestre { get; set; } = 1;
        public Carreras Carrera { get; set; }
    }
}
