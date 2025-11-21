#pragma once
#include "Grid.h"

class VariantRules
{
public:
	virtual bool isValid(const Grid& grid, int row, int col, int value) const = 0;
	virtual ~VariantRules() {}
};