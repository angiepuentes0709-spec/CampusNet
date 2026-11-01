# Entrega y revisión

1. Revisar el código y ejecutar la demostración y las pruebas con los comandos del README.
2. Crear en la cuenta de GitHub un repositorio público llamado CampusNet. Este proyecto es independiente del taller anterior DocuTrack.
3. Subir el contenido de esta carpeta, incluyendo Model, View, Controller, Pruebas, docs y .github. No subir bin ni obj.
4. Realizar el commit con la identidad real de Angie y revisar la pestaña Actions. El flujo debe completar compilación, pruebas y demostración.
5. Verificar que el repositorio se pueda consultar sin iniciar sesión y que el último commit esté dentro del plazo de entrega.
6. Entregar la URL pública en la plataforma antes del 8 de noviembre de 2026 a las 23:55.

No se ha publicado este proyecto desde la sesión de preparación. El archivo ZIP es una copia para descargar; la entrega exigida por el taller es el repositorio público.

## Preguntas para la sustentación

- ¿Por qué A -> B no implica B -> A? Porque cada arista representa una acción de seguimiento independiente.
- ¿Qué cambia respecto de DocuTrack? Se reemplaza el BST jerárquico por una lista de adyacencia que permite múltiples enlaces y ciclos.
- ¿Por qué hay un conjunto de visitados en BFS? Evita repetir usuarios y terminar en un bucle al recorrer ciclos.
- ¿Por qué un nodo negro no implica ciclo en DFS? Ya terminó su exploración y puede ser compartido por dos rutas de un grafo acíclico. Solo un enlace a un ancestro gris indica ciclo en esta búsqueda.
- ¿Por qué eliminar un usuario requiere revisar todas las listas? Puede aparecer como destino en las relaciones de cualquier otro usuario.
- ¿Es U11 aislado? No. Tiene entrada 0, pero salida 1.
- ¿Puede U12 alcanzarse a sí mismo? Sí; se acepta el camino de longitud cero y el BFS incluye su origen.
- ¿Influyente significa mejor usuario? No: aquí solo significa mayor grado de entrada.

## Revisión del trabajo anterior

El README actual de DocuTrack en la cuenta de Angie menciona a Sofía como integrante. Este proyecto nuevo se documenta individualmente según la indicación de Angie. No modifica ni reescribe los commits del taller anterior.
