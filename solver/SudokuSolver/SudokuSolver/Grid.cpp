#include "Grid.h"
#include <fstream>
#include <iostream>

Grid::Grid()
{
	for (int r = 0; r < 9; r++)
		for (int c = 0; c < 9; c++)
			cells[r][c] = 0;
}

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
			if (ch == '.') cells[r][c] = 0;
			else cells[r][c] = ch - '0';
		}
	}
	return true;
}

void Grid::print() const
{
	for (int r = 0; r < 9; r++)
	{
		for (int c = 0; c < 9; c++)
		{
			std::cout << cells[r][c] << " ";
		}
		std::cout << "\n";
	}
}

int Grid::get(int row, int col) const
{
	return cells[row][col];
}

int Grid::set(int row, int col, int value)
{
	return cells[row][col] = value;
}
