using ReticulasMovil.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Windows.Input;

namespace ReticulasMovil.ViewModel
{
    public class ReticulaViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Materia> Materias { get; set; } = new();
        List<Materia> Lista = new();
        Carreras carreraSeleccionada;

        public Materia Materia { get; set; } = null!;
        public IEnumerable<int> Semestres => Enumerable.Range(1, 9).ToList();
        public IEnumerable<Carreras> ListaCarreras => Enum.GetValues(typeof(Carreras)).Cast<Carreras>();
        public string Error { get; set; } = "";
        public Carreras CarreraSeleccionada { get => carreraSeleccionada; set { carreraSeleccionada = value;Actualizar(); } }
        public ICommand AgregarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand VerAgregarcommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        int indiceMateriaEditar;
        public ReticulaViewModel()
        {
            VerAgregarcommand = new Command(VerAgregar);
            CancelarCommand = new Command(Cancelar);
            AgregarCommand = new Command(Agregar);
            VerEliminarCommand = new Command<Materia>(VerEliminar);
            EliminarCommand = new Command(Eliminar);
            EditarCommand = new Command(Editar);
            VerEditarCommand = new Command<Materia>(VerEditar);
            Actualizar();
            Cargar();
        }

        private void VerEditar(Materia m)
        {
            var clon = new Materia
            {
                Clave = m.Clave,
                Carrera = m.Carrera,
                Semestre = m.Semestre,
                Nombre = m.Nombre
            };
            Materia = clon;
            indiceMateriaEditar = Lista.IndexOf(m);
            Error = "";
            PropertyChanged?.Invoke(this, new(nameof(Materia)));
            PropertyChanged?.Invoke(this, new(nameof(Error)));
            Shell.Current.GoToAsync("//Editar");
        }

        private void Editar()
        {
            if (Materia != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Materia.Clave))
                {
                    Error += "Escriba la clave de la materia\n";
                }
                if (string.IsNullOrWhiteSpace(Materia.Nombre))
                {
                    Error += "Escriba el nombre de la materia\n";
                }
                if (Lista[indiceMateriaEditar].Carrera != Materia.Carrera)
                {
                    if (Lista.Any(x => x.Clave == Materia.Clave && x.Carrera == Materia.Carrera))
                    {
                        Error += "La clave de la materia no debe repetirse en la misma carrera\n";
                    }
                }
                PropertyChanged?.Invoke(this, new(nameof(Error)));
                if (Error == "")
                {
                    Lista[indiceMateriaEditar] = Materia;
                    Guardar();
                    Actualizar();
                    Cancelar();
                }
            }
        }

        private void Eliminar()
        {
            if(Materia != null)
            {
                Lista.Remove(Materia);
                Guardar();
                Actualizar();
                Cancelar();
            }
        }

        private void VerEliminar(Materia m)
        {
            Materia = m;
            Error = "";
            PropertyChanged?.Invoke(this, new(nameof(Materia)));
            PropertyChanged?.Invoke(this, new(nameof(Error)));
            Shell.Current.GoToAsync("//Eliminar");
        }

        private void Agregar()
        {
            if (Materia != null)
            {
                Error = "";
                if (string.IsNullOrWhiteSpace(Materia.Clave))
                {
                    Error += "Escriba la clave de la materia\n";
                }
                if (string.IsNullOrWhiteSpace(Materia.Nombre))
                {
                    Error += "Escriba el nombre de la materia\n";
                }
                if (Lista.Any(x => x.Clave == Materia.Clave && x.Carrera == Materia.Carrera))
                {
                    Error += "La clave de la materia no debe repetirse en la misma carrera\n";
                }
                PropertyChanged?.Invoke(this, new(nameof(Error)));
                if (Error == "")
                {
                    Lista.Add(Materia);
                    Guardar();
                    Actualizar();
                    Cancelar();
                }
            }
        }

        private void Cancelar()
        {
            Error = "";
            PropertyChanged?.Invoke(this, new(nameof(Error)));
            Shell.Current.GoToAsync("//Lista");
        }

        private void VerAgregar()
        {
            Materia = new();
            PropertyChanged?.Invoke(this, new(nameof(Materia)));
            Shell.Current.GoToAsync("//Agregar");
        }

        private void Actualizar()
        {
            Materias.Clear();
            var info = Lista.Where(x => x.Carrera == CarreraSeleccionada).OrderBy(x => x.Semestre).ThenBy(x => x.Nombre);
            foreach (var item in info)
            {
                Materias.Add(item);
            }
        }

        private void Guardar()
        {
            var ruta = FileSystem.AppDataDirectory;
            File.WriteAllText(ruta + "/Materia.json", JsonSerializer.Serialize(Lista));
        }
        private void Cargar()
        {
            var ruta = FileSystem.AppDataDirectory;
            if (File.Exists(ruta + "/Materia.json"))
            {
                var datos = JsonSerializer.Deserialize<List<Materia>>(File.ReadAllText(ruta + "/Materia.json"));
                if (datos != null)
                {
                    Lista.AddRange(datos);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
