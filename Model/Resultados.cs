namespace CampusNet.Model;
public sealed record OperationResult(bool Exito, string Mensaje);
public sealed record DfsResult(IReadOnlyList<string> Orden, IReadOnlyList<IReadOnlyList<string>> Ciclos);
public sealed record Degree(Vertex Usuario, int Entrada, int Salida);
public sealed record GraphMetrics(int Vertices, int Aristas, double Densidad,
    IReadOnlyList<Degree> Grados, IReadOnlyList<Vertex> SinSeguidores,
    IReadOnlyList<Vertex> Aislados, IReadOnlyList<Vertex> Influyentes,
    IReadOnlyList<Vertex> Activos, int MaxEntrada, int MaxSalida);
