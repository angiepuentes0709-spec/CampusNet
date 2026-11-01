# Verificación del taller

Fecha: 1 de noviembre de 2026. Entorno: Windows, .NET SDK 8.0.427.

Compilación Release correcta: 0 errores y 0 advertencias. Las 12 pruebas del ejecutable independiente finalizaron correctamente. Se ejecutó además la aplicación compilada y se guardó su salida real en EJECUCION.txt. PRUEBAS.txt reproduce los mensajes obtenidos al ejecutar las pruebas.

| Requisito del anexo | Evidencia |
|---|---|
| C# Console App con MVC | Program.cs invoca solo al controlador; Model sin Console |
| Vertex, Edge y Graph | Archivos separados en Model |
| Lista de adyacencia dirigida | Dictionary<string, List<string>> en Graph |
| Al menos 12 usuarios y 18 aristas | 12 usuarios y 19 relaciones iniciales y finales |
| Roles variados | Estudiante, Profesor y Egresado |
| Dos usuarios con salida al menos 4 | U01 y U02, salida 4 |
| Dos usuarios sin seguidores | U11 y U12, entrada 0 |
| Ciclo dirigido | U01 -> U02 -> U03 -> U01; también se detecta otro ciclo |
| Tres BFS | U01: 10 alcanzados; U11: 11; U12: 1 |
| DFS completo | 12 usuarios descubiertos, incluyendo U12 |
| Consultas | Grados, influyentes, activos, sin seguidores, aislados y alcanzabilidad |
| CRUD | Alta de U13, alta y baja de aristas, actualización y baja de usuario |
| Limpieza de relaciones al borrar usuario | Eliminar U13 retira U02 -> U13 |
| Control de duplicados | Intentos rechazados y estado impreso |
| Métricas y análisis final | Sección 5 de EJECUCION.txt |
| README con autora y ejecución | README.md |
| Git público y commit dentro del plazo | Pendiente de publicación por la autora |

Los conteos de BFS incluyen el origen. Los ciclos reportados son los detectados por aristas de retroceso, no una enumeración exhaustiva de todos los ciclos simples. La densidad corresponde a un grafo dirigido simple sin autoaristas.

El proyecto no necesita paquetes externos. El SDK descargado para verificarlo permanece fuera de la entrega; debe instalarse un SDK compatible para compilar en otro equipo. El flujo de GitHub Actions está incluido, pero no se ha ejecutado en un repositorio remoto de CampusNet.
