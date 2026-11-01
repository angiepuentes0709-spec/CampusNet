namespace CampusNet.Model;
public enum Role { Estudiante, Profesor, Egresado }
public sealed record Vertex(string Id, string Nombre, Role Rol);
