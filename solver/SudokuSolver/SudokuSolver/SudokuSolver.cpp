#include "Grid.h"
#include "Solver.h"
#include "Rules.h"
#include "StandardRules.h"
#include "DiagonalRules.h"
#include <iostream>

int main()
{
    Grid grid; // the Sudoku grid

    /*
        Must use CORRECT ruleset with CORRECT puzzle .txt file
        - Standard: easy1.txt
        - Diagonal: diagonal1.txt

        If diagonal is used with standard etc. it will print:
        "No solution found."

        Will use procedurally generated puzzles later on.
    */
    StandardRules standard;
    Solver solver(&standard); // the actual Sudoku solver

    // Loads from the puzzle file
    if (!grid.loadFromFile("puzzles/easy1.txt"))
    {
        std::cout << "Failed to load puzzle file.\n";
        return 1;
    }

    // Shows the original puzzle
    std::cout << "Starting puzzle:\n";
    grid.print();

    // Checks if the rules functions correctly
    std::cout << "\nTesting the rules...\n";
    std::cout << Rules::isValid(grid, 0, 2, 1) << "\n";

    // For solving the puzzle
    if (solver.solve(grid))
    {
        // If successful, it'll print this
        std::cout << "\nSolved puzzle:\n";
        grid.print();

        std::cout << "\nSteps taken: " << solver.steps << "\n";
    }
    else
    {
        // Otherwise, it'll print this
        std::cout << "\nNo solution found.\n";
    }

    return 0;
}

