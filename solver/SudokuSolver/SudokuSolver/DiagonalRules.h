#pragma once
#include "VariantRules.h"
#include "Rules.h"

class DiagonalRules : public VariantRules
{
public:
	bool isValid(const Grid& grid, int row, int col, int value) const override;
};