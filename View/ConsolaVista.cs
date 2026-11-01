using CampusNet.Model;
namespace CampusNet.View;
public sealed class ConsolaVista
{
    public void Titulo(string texto) => Console.WriteLine($"\n=== {texto} ===");
    public void Texto(string texto) => Console.WriteLine(texto);
    public void Operacion(OperationResult r) => Console.WriteLine($"[{(r.Exito ? "OK" : "RECHAZADO")}] {r.Mensaje}");
    public void Grafo(IReadOnlyList<Vertex> usuarios, IReadOnlyList<Edge> relaciones)
    {
        foreach (var usuario in usuarios)
        {
            // Solo formatea las relaciones entregadas por el controlador.
            string vecinos = string.Join(", ", relaciones.Where(e => e.Origen == usuario.Id).Select(e => e.Destino));
            Console.WriteLine($"{usuario.Id} | {usuario.Nombre} | {usuario.Rol} -> [{vecinos}]");
        }
    }
    public void Bfs(string inicio, IReadOnlyList<string> orden) => Console.WriteLine(
        $"BFS desde {inicio}: {string.Join(" -> ", orden)} | alcanzados: {orden.Count} (incluye origen)");
    public void Dfs(DfsResult r)
    {
        Console.WriteLine($"DFS completo: {string.Join(" -> ", r.Orden)} | descubiertos: {r.Orden.Count}");
        if (r.Ciclos.Count == 0) Console.WriteLine("No se detectaron ciclos dirigidos.");
        foreach (var ciclo in r.Ciclos) Console.WriteLine($"Ciclo detectado: {string.Join(" -> ", ciclo)}");
    }
    public void Alcanzabilidad(string a, string b, bool resultado) => Console.WriteLine($"¿{a} puede llegar a {b}? {(resultado ? "Sí" : "No")}");
    private static string Nombres(IReadOnlyList<Vertex> usuarios) => usuarios.Count == 0 ? "ninguno" : string.Join(", ", usuarios.Select(u => $"{u.Id} ({u.Nombre})"));
    public void Metricas(GraphMetrics m)
    {
        Console.WriteLine($"Vértices: {m.Vertices}; aristas: {m.Aristas}; densidad dirigida: {m.Densidad:F4}");
        foreach (var g in m.Grados) Console.WriteLine($"{g.Usuario.Id}: entrada={g.Entrada}, salida={g.Salida}");
        Console.WriteLine($"Sin seguidores: {Nombres(m.SinSeguidores)}");
        Console.WriteLine($"Aislados (entrada y salida 0): {Nombres(m.Aislados)}");
        Console.WriteLine($"Influyentes (entrada máxima {m.MaxEntrada}): {Nombres(m.Influyentes)}");
        Console.WriteLine($"Activos (salida máxima {m.MaxSalida}): {Nombres(m.Activos)}");
    }
}
