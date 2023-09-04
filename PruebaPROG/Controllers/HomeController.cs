using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using a.Models;

namespace a.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Empleados()
    {
        // Hacemos el promedio de la antiguedad de los empleados
        // 1. Sumamos todas las antiguedades
        // 2. Las divimos por la cantidad que hay
        List<Empleado> _listaEmpleados = BD.GetEmpleados();
        ViewBag.ListaEmpleados = _listaEmpleados;
        if (_listaEmpleados.Count() > 0)
        {
            int suma = 0;
            foreach (Empleado empleado in _listaEmpleados) // 1
            {
                suma += empleado.AntiguedadEmpleado;
            }
            ViewBag.PromedioEmpleados = suma / _listaEmpleados.Count(); // 2
        }
        else
        {
            ViewBag.PromedioEmpleados = -1;
        }
        return View();
    }

    // Usamos "GET" porque estamos hablando de un link (Url.Action)
    [HttpGet]
    public IActionResult VerDetalleEmpleado(int IdEmpleado)
    {
        // Cargamos el viewbag con la informacion del empleado seleccionado
        ViewBag.Empleado = BD.GetEmpleadoById(IdEmpleado);
        return View();
    }

    public IActionResult CrearEmpleado()
    {
        // Cargamos el viewbag de la lista de los sectores
        ViewBag.ListaSectores = BD.GetSectores();
        return View();
    }

    // Usamos "POST" porque estamos hablando de un formulario (method="post")
    [HttpPost]
    public IActionResult GuardarEmpleado(string NombreEmpleado, string FotoEmpleado, int IdSector, int AntiguedadEmpleado)
    {
        // Reviso empleado por empelado y cuando concide el sector con el id que recibi sumo mi contador.
        // Si el contador es mayor o igual a tres tiro el error, sino lo agrego
        int cont = 0;
        foreach (Empleado empleado in BD.GetEmpleados())
        {
            if (empleado.IdSector == IdSector) // Chequeo si coincide
            {
                cont++; // Sumo el contador
            }
        }
        if (cont >= 3)
        {
            // Asigno el error y redireciono a CrearEmpleado para verlo
            ViewBag.ListaSectores = BD.GetSectores();
            ViewBag.Error = "Hay mas de 3 empleados";
            return View("CrearEmpleado", "Home");
        }
        else
        {
            // Buscamos el nombre del sector con el id que nos mandaron
            string NombreSector = "";
            foreach (Sector s in BD.GetSectores())
            {
                if (s.IdSector == IdSector) // Cuando coincide el id asignamos ese nombre
                {
                    NombreSector = s.NombreSector;
                }
            }

            // Creamos el objeto y se lo mandamos a la base de datos
            Empleado nuevoEmplado = new Empleado(NombreEmpleado, FotoEmpleado, IdSector, NombreSector, AntiguedadEmpleado);
            BD.InsertEmpleado(nuevoEmplado);
        }

        // Redireccionamos a Empleados
        return RedirectToAction("Empleados", "Home");
    }

    // Usamos "GET" porque estamos hablando de un link (Url.Action)
    [HttpGet]
    public IActionResult EliminarEmpleado(int IdEmpleado)
    {
        // Le pasamos el id a la funcion de BD pa que elimina al empleado
        // Y volvemos a la view de Empleados para ver los que quedaron
        BD.DeleteEmpleadoById(IdEmpleado);
        return RedirectToAction("Empleados", "Home");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
