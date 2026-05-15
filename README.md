# Console Chess

Two-player chess game running in the terminal. Written in C# (.NET 8).

## Run

```bash
dotnet run
```

## How to play

Moves use algebraic notation. White always goes first. The board flips each turn.

### Move notation

| Action | Format | Example |
|---|---|---|
| Move pawn | `[square]` | `e4` |
| Move piece | `[Piece][square]` | `Nf3`, `Rd1` |
| Capture with pawn | `[file]x[square]` | `exd5` |
| Capture with piece | `[Piece]x[square]` | `Bxf7` |
| Kingside castle | `0-0` | |
| Queenside castle | `0-0-0` | |
| Disambiguate by file | `[Piece][file][square]` | `Rae1` |
| Disambiguate by rank | `[Piece][rank][square]` | `R1e4` |

### Piece letters

| Letter | Piece |
|---|---|
| `T` | Rook |
| `C` | Knight |
| `A` | Bishop |
| `D` | Queen |
| `R` | King |
| *(none)* | Pawn |

### Special rules implemented

- **En passant** — available only on the turn immediately after a pawn advances two squares
- **Castling** — kingside (`0-0`) and queenside (`0-0-0`); blocked if king or rook has moved, or if any square between them is occupied or attacked
- **Pawn promotion** — on reaching the back rank, choose: Knight (`n`), Bishop (`b`), Rook (`r`), Queen (`q`)
- **Check detection** — illegal moves that leave your king in check are rejected
- **Checkmate detection** — game ends when the current player has no legal moves out of check

## Project structure

```
Program.cs          # Game loop, input parsing, move dispatch
Pieces/
  Piece.cs          # Base class
  SlidingPiece.cs   # Shared rook/bishop ray logic (base for Rook, Bishop, Queen)
  Pawn.cs
  Rook.cs
  Knight.cs
  Bishop.cs
  Queen.cs
  King.cs
```
