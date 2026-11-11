# Elo
Represents an Elo (rating), a method for calculating the relative skill levels of
players in competitor-versus-competitor games.

``` C#
var p0 = 1600.Elo();
var p1 = 1500.Elo();
var z = Elo.GetZScore(p0, p1); // 0.64 
```
