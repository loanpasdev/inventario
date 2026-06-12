[OPEN] demo-seed-error

## Síntoma
- La API falla al iniciar después de los cambios en el seeding de datos demo.

## Esperado
- La API debe arrancar, aplicar migraciones y sembrar productos demo sin errores.

## Hipótesis (falsables)
1. El seeder intenta acceder a una categoría/unidad que no existe (por código o nombre) y lanza excepción.
2. Hay una inconsistencia de datos (Products con CategoryId inválido) y al consultar `product.Category.Name` el join devuelve null.
3. El seeding intenta borrar productos pero hay restricciones/relaciones que impiden el delete (FK / restricciones).
4. El seeding se ejecuta antes de que existan UnitsOfMeasure (migraciones incompletas / error de conexión) y falla al resolver units.
5. La conexión apunta a una instancia SQL distinta a la que se está revisando, y el esquema real no coincide con lo esperado.

## Evidencia a recolectar
- Stacktrace exacto al arrancar la API (salida de consola).
- Connection string efectivo en runtime (Development).
- Conteo y estado de tablas clave: Categories, UnitsOfMeasure, Products.

## Evidencia recolectada
- Error de compilación:
  - `DemoDataSeeder.cs(24,17): error CS0136 ... 'existingCategoryNames' ...`

## Resultado
- Corregido el conflicto de nombres (variable shadowing) renombrando la variable a `existingProductCategoryNames`.
- `dotnet build` OK.
- La API inicia correctamente en `http://localhost:5129`.

