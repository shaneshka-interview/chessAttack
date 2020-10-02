# Задача

-   Создать класс имплементящий интерфейс IAttackCounter
-   Метод CountUnderAttack должен считать количество клеток шахматной доски находящихся под ударом шахматной фигуры
-   На данный момент должны поддерживаться три фигуры - Rook (ладья), Bishop (слон) и Knight (конь)
-   Размер доски задается аргументом boardSize
-   Кол-во препятствий и размер доски может достигать 100 000 эллементов
-   начальная точка фигуры для которой считаем число клеток под ударом - startCoords
-   координаты препятствий находятся в массиве obstacles
-   все координаты считаются начиная с 1 от левого нижнего угла
-   можно (и желательно) добавить дополнительные тесты для проверки в AttackSolverTest
-   `namespace AttackSolver`  переименуйте в вашу фамилию, например `namespace Ivanov`
-   интерфейс править не надо
-   ответ необходимо послать в виде ZIP архива в скайп DIMSAZ

# Примеры

## Обозначения

O - obstacle
R - Rook
B - Bishop
K - Knight
. - empty cell

## Rook

cmType=Rook, boardSize={3,2}, startCoords={1, 1}, obstacles={{2,2}, {3, 1}}

```
.O.
R.O
```

вывод: 2

## Bishop

cmType=Bishop, boardSize={4,5}, startCoords={2, 2}, obstacles={{1,1}, {1, 3}}

```
....
....
O...
.B..
O...
```

вывод: 3