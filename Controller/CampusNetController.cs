using CampusNet.Model;
using CampusNet.View;
namespace CampusNet.Controller;
public sealed class CampusNetController
{
    private readonly Graph grafo = new();
    private readonly ConsolaVista vista = new();
    private void Mostrar() => vista.Grafo(grafo.Usuarios, grafo.Relaciones);
    private void Cambio(OperationResult resultado) { vista.Operacion(resultado); Mostrar(); }
    public void Ejecutar()
    {
        vista.Titulo("CampusNet - Programación III - Angie Valeria Puentes Garcia");
        vista.Titulo("1. Construcción del grafo dirigido");
        (string Id, string Nombre, Role Rol)[] usuarios = {
            ("U01", "Ana", Role.Estudiante), ("U02", "Bruno", Role.Profesor),
            ("U03", "Camila", Role.Egresado), ("U04", "Diego", Role.Estudiante),
            ("U05", "Elena", Role.Profesor), ("U06", "Felipe", Role.Egresado),
            ("U07", "Gabriela", Role.Estudiante), ("U08", "Hugo", Role.Profesor),
            ("U09", "Isabel", Role.Egresado), ("U10", "Julián", Role.Estudiante),
            ("U11", "Karen", Role.Profesor), ("U12", "Luis", Role.Egresado)
        };
        foreach (var u in usuarios) vista.Operacion(grafo.AgregarUsuario(u.Id, u.Nombre, u.Rol));
        (string A, string B)[] relaciones = {
            ("U01","U02"), ("U01","U03"), ("U01","U04"), ("U01","U05"),
            ("U02","U03"), ("U02","U04"), ("U02","U05"), ("U02","U06"),
            ("U03","U01"), ("U03","U07"), ("U04","U05"), ("U04","U08"),
            ("U05","U06"), ("U06","U07"), ("U07","U08"), ("U08","U09"),
            ("U09","U10"), ("U10","U05"), ("U11","U02")
        };
        foreach (var (a,b) in relaciones) vista.Operacion(grafo.AgregarRelacion(a,b));
        Mostrar(); vista.Metricas(grafo.Metricas());
        vista.Titulo("2. Recorridos");
        foreach (string inicio in new[] { "U01", "U11", "U12" }) vista.Bfs(inicio, grafo.Bfs(inicio));
        vista.Dfs(grafo.DfsCompleto());
        vista.Titulo("3. Consultas de alcanzabilidad");
        foreach (var (a,b) in new[] { ("U11","U10"), ("U01","U11"), ("U12","U01") })
            vista.Alcanzabilidad(a,b,grafo.PuedeLlegar(a,b));
        vista.Titulo("4. CRUD y validaciones - estructura después de cada operación");
        Cambio(grafo.AgregarUsuario("U13","María",Role.Estudiante));
        Cambio(grafo.AgregarRelacion("U13","U01"));
        Cambio(grafo.AgregarRelacion("U02","U13"));
        Cambio(grafo.ActualizarUsuario("U13","María Torres",Role.Egresado));
        Cambio(grafo.EliminarRelacion("U13","U01"));
        // Eliminar U13 también debe limpiar U02 -> U13.
        Cambio(grafo.EliminarUsuario("U13"));
        Cambio(grafo.AgregarUsuario("U01","Otra Ana",Role.Profesor));
        Cambio(grafo.AgregarRelacion("U01","U02"));
        Cambio(grafo.AgregarRelacion("U01","U99"));
        vista.Titulo("5. Estructura y métricas finales");
        Mostrar(); vista.Metricas(grafo.Metricas()); vista.Dfs(grafo.DfsCompleto());
        vista.Texto("Análisis: las operaciones temporales restauraron las 19 relaciones y los 12 usuarios iniciales.");
        vista.Texto("U01 y U02 siguen a 4 usuarios cada uno. U05 recibe 4 seguimientos y tiene el mayor grado de entrada.");
        vista.Texto("U11 no tiene seguidores, pero sigue a U02: no es aislado. U12 sí es aislado y su BFS solo alcanza su origen.");
        vista.Texto("El ciclo U01 -> U02 -> U03 -> U01 impide un orden topológico. La red no es fuertemente conexa.");
        vista.Texto("La influencia se mide aquí solo por seguidores; no implica prestigio ni calidad académica.");
    }
}
