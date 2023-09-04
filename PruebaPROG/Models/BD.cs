using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

public static class BD
{
    private static string _connectionString = @"Server=.; DataBase=BDEmpresa;Trusted_Connection=true";

    public static List<Empleado> GetEmpleados()
    {
        List<Empleado> Lista = null;
        string SQL = "SELECT * FROM Empleados";
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            Lista = db.Query<Empleado>(SQL).ToList();
        }
        return Lista;
    }
    public static Empleado GetEmpleadoById(int Id)
    {
        Empleado item = null;
        string SQL = "SELECT * FROM Empleados E INNER JOIN Sectores S ON S.IdSector=E.IdSector ";
        SQL += " WHERE IdEmpleado=@pId";
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            item = db.QueryFirstOrDefault<Empleado>(SQL, new { pId = Id });
        }
        return item;
    }
    public static int GetEmpleadosByIdSector(int IdSector)
    {
        int item = 0;
        string SQL = "SELECT COUNT(IdEmpleado) FROM Empleados WHERE IdSector=@pId ";
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            item = (int)db.ExecuteScalar(SQL, new { pId = IdSector });
        }
        return item;
    }

    public static void DeleteEmpleadoById(int Id)
    {
        string SQL = "DELETE FROM Empleados WHERE IdEmpleado=@pId";
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            db.Execute(SQL, new { pId = Id });
        }
    }
    public static List<Sector> GetSectores()
    {
        List<Sector> Lista = null;
        string SQL = "SELECT * FROM Sectores";
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            Lista = db.Query<Sector>(SQL).ToList();
        }
        return Lista;
    }
    public static void InsertEmpleado(Empleado item)
    {
        string SQL = "INSERT INTO Empleados(NombreEmpleado, FotoEmpleado, AntiguedadEmpleado, IdSector)";
        SQL += " VALUES (@pNombreEmpleado, @pFotoEmpleado, @pAntiguedadEmpleado, @pIdSector) ";
        using (SqlConnection db = new SqlConnection(_connectionString))
        {
            db.Execute(SQL, new
            {
                pNombreEmpleado = item.NombreEmpleado,
                pFotoEmpleado = item.FotoEmpleado,
                pAntiguedadEmpleado = item.AntiguedadEmpleado,
                pIdSector = item.IdSector
            });
        }
    }
}