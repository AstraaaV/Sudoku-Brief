#pragma once
#include <string>

class Grid
{
public:
	Grid();
	bool loadFromFile(const std::string& filepath);
	void print() const;

	int get(int row, int col) const;
	int set(int row, int col, int value);

private:
	int cells[9][9];
};