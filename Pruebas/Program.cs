using CampusNet.Model;
using CampusNet.Controller;
var pruebas = new List<(string Nombre, Action Ejecutar)>();
void Verificar(bool condicion, string mensaje = "Condición incumplida") { if (!condicion) throw new Exception(mensaje); }
Graph Crear(params string[] ids) { var g = new Graph(); foreach (var id in ids) g.AgregarUsuario(id,id,Role.Estudiante); return g; }
pruebas.Add(("Validación y duplicados de usuarios", () => {
 var g=Crear("A"); Verificar(!g.AgregarUsuario(" A ","Otro",Role.Profesor).Exito);
 Verificar(!g.AgregarUsuario(" ","Nombre",Role.Profesor).Exito);
 Verificar(!g.AgregarUsuario("B"," ",Role.Profesor).Exito);
 Verificar(!g.AgregarUsuario("B","B",(Role)99).Exito); Verificar(g.VertexCount==1);
}));
pruebas.Add(("Relaciones dirigidas, duplicados y extremos", () => {
 var g=Crear("A","B"); Verificar(g.AgregarRelacion("A","B").Exito);
 Verificar(!g.AgregarRelacion("A","B").Exito); Verificar(!g.AgregarRelacion("A","X").Exito);
 Verificar(!g.AgregarRelacion("A","A").Exito); Verificar(!g.PuedeLlegar("B","A"));
 Verificar(g.AgregarRelacion("B","A").Exito && g.EdgeCount==2);
}));
pruebas.Add(("BFS por niveles y sin repetir en ciclo", () => {
 var g=Crear("A","B","C","D","E");
 foreach(var (a,b) in new[]{("A","B"),("A","C"),("B","D"),("C","D"),("D","A")}) g.AgregarRelacion(a,b);
 Verificar(g.Bfs("A").SequenceEqual(new[]{"A","B","C","D"}));
 Verificar(g.Bfs("E").SequenceEqual(new[]{"E"}));
 Verificar(g.PuedeLlegar("A","A") && !g.PuedeLlegar("A","E") && !g.PuedeLlegar("X","A"));
 try { g.Bfs("X"); throw new Exception("Debía rechazar origen inexistente"); } catch(ArgumentException) { }
}));
pruebas.Add(("DFS completo y ciclo real", () => {
 var g=Crear("A","B","C","D"); g.AgregarRelacion("A","B");g.AgregarRelacion("B","C");g.AgregarRelacion("C","A");
 var d=g.DfsCompleto(); Verificar(d.Orden.SequenceEqual(new[]{"A","B","C","D"}));
 Verificar(d.Ciclos.Count==1 && d.Ciclos[0].SequenceEqual(new[]{"A","B","C","A"}));
}));
pruebas.Add(("DFS no confunde arista a nodo terminado con ciclo", () => {
 var g=Crear("A","B","C","D"); foreach(var (a,b) in new[]{("A","B"),("A","C"),("B","D"),("C","D")})g.AgregarRelacion(a,b);
 Verificar(g.DfsCompleto().Ciclos.Count==0);
}));
pruebas.Add(("Actualizar conserva Id y relaciones; rechazos atómicos", () => {
 var g=Crear("A","B");g.AgregarRelacion("A","B");
 Verificar(g.ActualizarUsuario("A","Ana",Role.Profesor).Exito);
 Verificar(!g.ActualizarUsuario("A","",Role.Egresado).Exito);
 Verificar(!g.ActualizarUsuario("X","X",Role.Egresado).Exito);
 Verificar(g.Usuarios[0].Nombre=="Ana" && g.Usuarios[0].Rol==Role.Profesor && g.EdgeCount==1);
}));
pruebas.Add(("Eliminar usuario limpia aristas entrantes y salientes", () => {
 var g=Crear("A","B","C");g.AgregarRelacion("A","B");g.AgregarRelacion("B","A");g.AgregarRelacion("C","B");g.AgregarRelacion("A","C");
 Verificar(g.EliminarUsuario("B").Exito && g.VertexCount==2 && g.EdgeCount==1);
 Verificar(g.Relaciones.All(e=>e.Origen!="B" && e.Destino!="B"));
 Verificar(!g.EliminarUsuario("B").Exito && !g.EliminarRelacion("C","A").Exito);
 Verificar(g.EliminarRelacion("A","C").Exito && g.EdgeCount==0);
}));
pruebas.Add(("Métricas, empates, sin seguidores y aislados", () => {
 var g=Crear("A","B","C","D");g.AgregarRelacion("A","C");g.AgregarRelacion("B","C");
 var m=g.Metricas();Verificar(m.MaxEntrada==2 && m.Influyentes.Single().Id=="C");
 Verificar(m.Activos.Select(u=>u.Id).SequenceEqual(new[]{"A","B"}));
 Verificar(m.SinSeguidores.Count==3 && m.Aislados.Single().Id=="D");
 Verificar(Math.Abs(m.Densidad-2.0/12)<1e-10);
 Verificar(m.Grados.Sum(x=>x.Entrada)==g.EdgeCount && m.Grados.Sum(x=>x.Salida)==g.EdgeCount);
}));
pruebas.Add(("Grafo vacío y usuarios aislados", () => {
 var g=new Graph();Verificar(g.Metricas().Densidad==0 && g.DfsCompleto().Orden.Count==0);
 g.AgregarUsuario("A","A",Role.Egresado);Verificar(g.Metricas().Densidad==0 && g.Metricas().Aislados.Count==1);
 Verificar(g.PuedeLlegar("A","A"));
}));
pruebas.Add(("Las consultas no exponen listas internas modificables", () => {
 var g=Crear("A","B");g.AgregarRelacion("A","B");
 var vecinos=(string[])g.Vecinos("A"); vecinos[0]="X";
 Verificar(g.Vecinos("A").Single()=="B");
}));
pruebas.Add(("Modelo no escribe en consola", () => {
 var salida=Console.Out;using var buffer=new StringWriter();
 try { Console.SetOut(buffer);var g=Crear("A","B");g.AgregarRelacion("A","B");g.Bfs("A");g.DfsCompleto();g.Metricas();g.EliminarUsuario("A"); }
 finally { Console.SetOut(salida); }
 Verificar(buffer.ToString().Length==0);
}));
pruebas.Add(("Demostración completa y requisitos de construcción", () => {
 var salida=Console.Out;using var buffer=new StringWriter();
 try { Console.SetOut(buffer);new CampusNetController().Ejecutar(); } finally { Console.SetOut(salida); }
 string texto=buffer.ToString();Verificar(texto.Contains("Vértices: 12; aristas: 19"));
 Verificar(texto.Contains("BFS desde U01: U01 -> U02 -> U03 -> U04 -> U05 -> U06 -> U07 -> U08 -> U09 -> U10 | alcanzados: 10"));
 Verificar(texto.Contains("BFS desde U11: U11 -> U02 -> U03 -> U04 -> U05 -> U06 -> U01 -> U07 -> U08 -> U09 -> U10 | alcanzados: 11"));
 Verificar(texto.Contains("BFS desde U12: U12 | alcanzados: 1"));
 Verificar(texto.Contains("U01: entrada=1, salida=4") && texto.Contains("U02: entrada=2, salida=4"));
 Verificar(texto.Contains("Ciclo detectado: U01 -> U02 -> U03 -> U01"));
 Verificar(texto.Contains("relaciones incidentes eliminadas: 1"));
 Verificar(texto.Contains("Sin seguidores: U11 (Karen), U12 (Luis)"));
}));
int fallos=0;
foreach(var (nombre,ejecutar) in pruebas) { try { ejecutar();Console.WriteLine($"[OK] {nombre}"); } catch(Exception e) { fallos++;Console.WriteLine($"[FALLO] {nombre}: {e.Message}"); } }
Console.WriteLine($"Resultado: {pruebas.Count-fallos}/{pruebas.Count} pruebas aprobadas.");
return fallos==0 ? 0 : 1;
