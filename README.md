# CampusNet — Grafos dirigidos con MVC en C#

Programación III · Universidad de Manizales · Actividad colaborativa N.º 2.

Aplicación de consola que representa seguimientos entre usuarios académicos. Una arista A -> B significa que A sigue a B. Ejecuta automáticamente todos los casos del taller, sin entrada manual, base de datos ni paquetes NuGet externos.

## Integrante y responsabilidades

| Integrante | Responsabilidades |
|---|---|
| Angie Valeria Puentes Garcia | Trabajo individual: modelo del grafo, CRUD, BFS, DFS, métricas, vista, controlador, pruebas y documentación. |

Este taller se presenta individualmente. Los commits deben corresponder a aportes reales de la autora.

## Ejecución

Requisito: .NET SDK 8.0. Abrir una terminal en la carpeta que contiene `CampusNet.csproj`.

```bash
dotnet build CampusNet.csproj --configuration Release
dotnet run --project CampusNet.csproj --configuration Release
```

Pruebas independientes, sin frameworks externos:

```bash
dotnet run --project Pruebas/CampusNet.Pruebas.csproj --configuration Release
```

En Visual Studio, abrir `CampusNet.csproj` y ejecutar con Ctrl+F5. Para guardar evidencia desde PowerShell:

```powershell
dotnet run --project CampusNet.csproj --configuration Release | Out-File -Encoding utf8 ejecucion.txt
```

## Arquitectura MVC

| Archivo | Responsabilidad |
|---|---|
| `Program.cs` | Solo invoca al controlador. |
| `Model/Vertex.cs` | Usuario inmutable con Id, nombre y rol. |
| `Model/Edge.cs` | Relación dirigida entre identificadores. |
| `Model/Graph.cs` | Lista de adyacencia, CRUD, recorridos y métricas; no usa Console. |
| `Model/Resultados.cs` | Resultados y métricas que se trasladan a la vista. |
| `View/ConsolaVista.cs` | Presenta datos ya calculados; no modifica ni recorre el grafo. |
| `Controller/CampusNetController.cs` | Crea los casos de demostración y comunica modelo con vista. |
| `Pruebas/` | Verificaciones del modelo y de la ejecución completa. |
| `docs/` | Evidencias, análisis y guía de entrega. |

Se conserva la organización general del taller anterior DocuTrack de la autora. CampusNet implementa un dominio y una estructura nuevos: un grafo dirigido, en lugar de un árbol binario de búsqueda.

## Reglas del grafo

- Representación: `Dictionary<string, List<string>>`, con usuarios en un diccionario separado y orden de inserción explícito.
- El Id es único, se recortan espacios de los extremos y se distinguen mayúsculas (`U01` y `u01` serían claves distintas).
- Nombre no vacío y rol válido: Estudiante, Profesor o Egresado.
- Se rechazan usuarios y aristas duplicadas y relaciones con extremos inexistentes.
- No se permite seguirse a sí mismo; es una decisión de este modelo de red social.
- Eliminar un usuario elimina todas sus relaciones entrantes y salientes.
- Actualizar nombre o rol conserva el Id y las relaciones. Los cambios inválidos no alteran el estado.
- La relación inversa se agrega por separado: seguir no implica ser seguido.
- Los datos se reconstruyen en memoria en cada ejecución; no hay persistencia.

## Casos ejecutados

1. Se registran 12 usuarios con los tres roles y 19 relaciones; se imprimen usuarios, adyacencias y grados.
2. BFS desde U01, U11 y U12: muestra orden y cantidad, incluyendo el origen.
3. DFS completo: visita todas las componentes y detecta ciclos mediante vértices grises en la pila de recursión.
4. Consultas de influencia, actividad, usuarios sin seguidores, aislados y alcanzabilidad dirigida.
5. CRUD: agrega U13, agrega dos relaciones, actualiza nombre y rol, elimina una relación y elimina U13 con limpieza de la relación entrante restante. Imprime después de cada operación.
6. Rechaza usuario duplicado, arista duplicada y extremo inexistente; imprime el estado conservado.
7. Presenta estructura, métricas, DFS y análisis finales.

## Resultados que se deben observar

- 12 vértices y 19 aristas tanto al inicio como al final; el CRUD usa un usuario temporal y restaura el grafo inicial.
- U01 y U02 tienen grado de salida 4: son los más activos.
- U05 tiene grado de entrada 4: es la usuaria más influyente según esta métrica.
- U11 y U12 no tienen seguidores. Solo U12 está aislado, pues U11 sí sigue a U02.
- BFS desde U01 alcanza 10 usuarios; desde U11, 11; desde U12, únicamente a U12.
- Un ciclo detectado es U01 -> U02 -> U03 -> U01. El DFS informa ciclos mediante aristas de retroceso, sin enumerar todos los ciclos simples posibles.
- U11 alcanza a U10; U01 no alcanza a U11; U12 no alcanza a U01.
- Densidad dirigida sin autoaristas: 19 / (12 × 11) = 0,1439.

Las evidencias reales y el estado de verificación se documentan en `docs/VERIFICACION.md`, `docs/EJECUCION.txt` y `docs/PRUEBAS.txt`.

## Análisis y complejidad

BFS usa una cola y marca al encolar para evitar duplicados. DFS usa estados blanco, gris y negro; una arista hacia un vértice gris demuestra un ciclo dirigido. Se inicia una nueva búsqueda en cada vértice todavía blanco para incluir los desconectados.

Sin seguidores significa entrada 0; aislado significa entrada y salida 0. La alcanzabilidad no es simétrica. La red no es fuertemente conexa porque U12 no puede comunicarse con otros usuarios. La existencia de ciclos impide un orden topológico.

Con V usuarios y E relaciones: BFS es O(V + E); el núcleo del DFS es O(V + E), más el costo de copiar los ciclos reportados (hasta O(V) por arista de retroceso). Las métricas y la eliminación de un usuario son O(V + E). La consulta y actualización de un usuario son O(1) en promedio. Agregar o eliminar una relación cuesta O(grado de salida del origen) por usar listas y comprobar duplicados. La memoria del grafo es O(V + E). El DFS recursivo es apropiado para este taller; una red muy grande requeriría considerar una pila explícita.

## Entrega en GitHub

La fecha de cierre indicada por la plataforma es el 8 de noviembre de 2026 a las 23:55. Consultar `docs/ENTREGA.md`. La carpeta está preparada para publicación, pero todavía no se ha creado ni actualizado un repositorio remoto de CampusNet. No se atribuyen commits ni un enlace público inexistentes.
