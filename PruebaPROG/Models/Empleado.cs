public class Empleado
{
    public int IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public string FotoEmpleado { get; set; }
    public int IdSector { get; set; }
    public string NombreSector { get; set; }
    public int AntiguedadEmpleado { get; set; }

    public Empleado() { }

    public Empleado(string nombre, string foto, int idsector, string nombresector, int antiguedad)
    {
        NombreEmpleado = nombre;
        FotoEmpleado = foto;
        IdSector = idsector;
        NombreSector = nombresector;
        AntiguedadEmpleado = antiguedad;
    }
}