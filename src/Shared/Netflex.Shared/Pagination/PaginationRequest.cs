﻿namespace Netflex.Shared.Pagination;

public record PaginationRequest(int PageIndex = 1, int PageSize = 10);
