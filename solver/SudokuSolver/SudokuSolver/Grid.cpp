#include "Grid.h"
#include <fstream>
#include <iostream>

// Makes sure the grid starts as 0 aka empty
Grid::Grid()
{
	for (int r = 0; r < 9; r++)
		for (int c = 0; c < 9; c++)
			cells[r][c] = 0;
}

// This reads a puzzle from a .txt file and returns false if unable to be opened 
bool Grid::loadFromFile(const std::string& filepath)
{
	std::ifstream file(filepath);

	if (!file.is_open()) return false;

	for (int r = 0; r < 9; r++)
	{
		for (int c = 0; c < 9; c++)
		{
			char ch;
			file >> ch;
			if (ch == '.') cells[r][c] = 0; // empty cell
			else cells[r][c] = ch - '0'; // converts char to number
		}
	}
	return true;
}

// Prints the grid to the console
void Grid::print() const
{
	for (int r = 0; r < 9; r++)
	{
		for (int c = 0; c < 9; c++)
		{
			std::cout << cells[r][c] << " ";
		}
		std::cout << "\n"; // new row
	}
}

// Returns the current value
int Grid::get(int row, int col) const
{
	return cells[row][col];
}

// Updates the value
int Grid::set(int row, int col, int value)
{
	return cells[row][col] = value;
}
