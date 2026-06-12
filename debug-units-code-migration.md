[OPEN] units-code-migration

## Sintoma
- La API falla al aplicar la migracion `20260611094000_AddMasterStatusesAndCurrencies`.

## Esperado
- La API debe aplicar la migracion y arrancar sin errores.

## Hipotesis
1. El `ALTER COLUMN UnitsOfMeasure.Code nvarchar(3)` falla porque el indice `IX_UnitsOfMeasure_Code` sigue activo durante la alteracion.
2. El orden de operaciones de la migracion es incorrecto: primero deberia soltarse el indice, luego ajustar datos y al final recrear el indice.
3. Existen valores de `Code` con longitud mayor a 3 y EF intenta alterar la columna antes de que los `UPDATE` reduzcan esos valores.
4. El error no esta en `Currencies`; el punto real de quiebre es la modificacion de `UnitsOfMeasure.Code`.

## Evidencia inicial
- Stacktrace del usuario:
  - `The index 'IX_UnitsOfMeasure_Code' is dependent on column 'Code'`
  - `ALTER TABLE ALTER COLUMN Code failed because one or more objects access this column`

## Analisis
- Hipotesis 1 confirmada: el indice `IX_UnitsOfMeasure_Code` bloqueaba el `ALTER COLUMN`.
- Hipotesis 2 confirmada: la migracion necesitaba soltar y recrear el indice alrededor del cambio de longitud.
- Hipotesis 3 no confirmada con la evidencia actual.
- Hipotesis 4 confirmada: el fallo real no era `Currencies`, sino `UnitsOfMeasure.Code`.

## Fix aplicado
- Se agrego `DropIndex("IX_UnitsOfMeasure_Code")` antes de `AlterColumn`.
- Se agrego `CreateIndex("IX_UnitsOfMeasure_Code")` al final del `Up`.
- Se reflejo el mismo orden en `Down`.

## Verificacion
- Diagnosticos del archivo: sin errores.
- `dotnet build src/InventoryManagement.Api/InventoryManagement.Api.csproj`: OK.
