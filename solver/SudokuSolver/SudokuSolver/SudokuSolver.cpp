#include "Grid.h"
#include "Solver.h"
#include "Rules.h"
#include "StandardRules.h"
#include "DiagonalRules.h"
#include <iostream>

int main()
{
    Grid grid; // the Sudoku grid
    DiagonalRules diagonal;
    Solver solver(&diagonal); // the actual Sudoku solver

    // Loads from the puzzle file
    if (!grid.loadFromFile("puzzles/diagonal1.txt"))
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
    }
    else
    {
        // Otherwise, it'll print this
        std::cout << "\nNo solution found.\n";
    }

    return 0;
}

