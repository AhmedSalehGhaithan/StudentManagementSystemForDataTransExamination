﻿namespace StudentManagementSystem.Application.Dtos.AccountDTOs.Response;

public record LoginResponse(bool Flag = false, string Message = null!, string Token = null!, string RefreshToken = null!);