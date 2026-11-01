namespace CampusNet.Model;

public sealed class Graph
{
    private readonly Dictionary<string, Vertex> vertices = new(StringComparer.Ordinal);
    private readonly Dictionary<string, List<string>> adyacencias = new(StringComparer.Ordinal);
    // Orden explícito para que las evidencias sean reproducibles.
    private readonly List<string> orden = new();
    private static string Normalizar(string? id) => id?.Trim() ?? "";
    public IReadOnlyList<Vertex> Usuarios => orden.Select(id => vertices[id]).ToArray();
    public IReadOnlyList<Edge> Relaciones => orden.SelectMany(a => adyacencias[a].Select(b => new Edge(a, b))).ToArray();
    public int VertexCount => vertices.Count;
    public int EdgeCount => adyacencias.Values.Sum(lista => lista.Count);
    public IReadOnlyList<string> Vecinos(string id) => adyacencias[Exigir(id)].ToArray();
    public bool Contiene(string id) => vertices.ContainsKey(Normalizar(id));
    private string Exigir(string id)
    {
        id = Normalizar(id);
        if (!vertices.ContainsKey(id)) throw new ArgumentException($"No existe el usuario '{id}'.", nameof(id));
        return id;
    }
    public OperationResult AgregarUsuario(string id, string nombre, Role rol)
    {
        id = Normalizar(id); nombre = nombre?.Trim() ?? "";
        if (id.Length == 0 || nombre.Length == 0 || !Enum.IsDefined(rol))
            return new(false, "Id, nombre y rol deben ser válidos.");
        if (vertices.ContainsKey(id)) return new(false, $"Usuario duplicado: {id}.");
        vertices.Add(id, new Vertex(id, nombre, rol)); adyacencias.Add(id, new()); orden.Add(id);
        return new(true, $"Usuario agregado: {id} ({nombre}, {rol}).");
    }
    public OperationResult ActualizarUsuario(string id, string nombre, Role rol)
    {
        id = Normalizar(id); nombre = nombre?.Trim() ?? "";
        if (!vertices.ContainsKey(id)) return new(false, $"No existe el usuario {id}.");
        if (nombre.Length == 0 || !Enum.IsDefined(rol)) return new(false, "Nombre o rol inválido.");
        vertices[id] = new Vertex(id, nombre, rol);
        return new(true, $"Usuario actualizado: {id} ({nombre}, {rol}).");
    }
    public OperationResult EliminarUsuario(string id)
    {
        id = Normalizar(id);
        if (!vertices.ContainsKey(id)) return new(false, $"No existe el usuario {id}.");
        int antes = EdgeCount;
        adyacencias.Remove(id);
        foreach (var lista in adyacencias.Values) lista.Remove(id);
        vertices.Remove(id); orden.Remove(id);
        return new(true, $"Usuario eliminado: {id}; relaciones incidentes eliminadas: {antes - EdgeCount}.");
    }
    public OperationResult AgregarRelacion(string origen, string destino)
    {
        origen = Normalizar(origen); destino = Normalizar(destino);
        if (!vertices.ContainsKey(origen) || !vertices.ContainsKey(destino))
            return new(false, "Ambos usuarios deben existir.");
        if (origen == destino) return new(false, "No se permite seguirse a sí mismo.");
        if (adyacencias[origen].Contains(destino)) return new(false, $"Relación duplicada: {origen} -> {destino}.");
        adyacencias[origen].Add(destino);
        return new(true, $"Relación agregada: {origen} -> {destino}.");
    }
    public OperationResult EliminarRelacion(string origen, string destino)
    {
        origen = Normalizar(origen); destino = Normalizar(destino);
        if (!vertices.ContainsKey(origen) || !vertices.ContainsKey(destino)) return new(false, "Ambos usuarios deben existir.");
        return adyacencias[origen].Remove(destino)
            ? new(true, $"Relación eliminada: {origen} -> {destino}.")
            : new(false, $"No existe la relación {origen} -> {destino}.");
    }
    public IReadOnlyList<string> Bfs(string inicio)
    {
        inicio = Exigir(inicio);
        var visitados = new HashSet<string> { inicio };
        var pendientes = new Queue<string>(); pendientes.Enqueue(inicio);
        var resultado = new List<string>();
        while (pendientes.Count > 0)
        {
            string actual = pendientes.Dequeue(); resultado.Add(actual);
            foreach (string vecino in adyacencias[actual])
                if (visitados.Add(vecino)) pendientes.Enqueue(vecino);
        }
        return resultado.ToArray();
    }
    public bool PuedeLlegar(string origen, string destino)
    {
        origen = Normalizar(origen); destino = Normalizar(destino);
        return Contiene(origen) && Contiene(destino) && Bfs(origen).Contains(destino);
    }
    public DfsResult DfsCompleto()
    {
        // Blanco: sin visitar; gris: en la pila; negro: terminado.
        var color = orden.ToDictionary(id => id, _ => 0);
        var descubrimiento = new List<string>(); var pila = new List<string>();
        var ciclos = new List<IReadOnlyList<string>>();
        void Visitar(string actual)
        {
            color[actual] = 1; descubrimiento.Add(actual); pila.Add(actual);
            foreach (string vecino in adyacencias[actual])
            {
                if (color[vecino] == 0) Visitar(vecino);
                else if (color[vecino] == 1)
                    ciclos.Add(pila.Skip(pila.IndexOf(vecino)).Append(vecino).ToArray());
            }
            pila.RemoveAt(pila.Count - 1); color[actual] = 2;
        }
        foreach (string id in orden) if (color[id] == 0) Visitar(id);
        return new(descubrimiento.ToArray(), ciclos.ToArray());
    }
    public GraphMetrics Metricas()
    {
        var entrada = orden.ToDictionary(id => id, _ => 0);
        foreach (var lista in adyacencias.Values) foreach (string id in lista) entrada[id]++;
        var grados = orden.Select(id => new Degree(vertices[id], entrada[id], adyacencias[id].Count)).ToArray();
        int maxEntrada = grados.Select(g => g.Entrada).DefaultIfEmpty(0).Max();
        int maxSalida = grados.Select(g => g.Salida).DefaultIfEmpty(0).Max();
        return new(VertexCount, EdgeCount, VertexCount < 2 ? 0 : (double)EdgeCount / ((double)VertexCount * (VertexCount - 1)),
            grados, grados.Where(g => g.Entrada == 0).Select(g => g.Usuario).ToArray(),
            grados.Where(g => g.Entrada == 0 && g.Salida == 0).Select(g => g.Usuario).ToArray(),
            grados.Where(g => g.Entrada == maxEntrada).Select(g => g.Usuario).ToArray(),
            grados.Where(g => g.Salida == maxSalida).Select(g => g.Usuario).ToArray(), maxEntrada, maxSalida);
    }
}
