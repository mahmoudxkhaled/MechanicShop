using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domain.Common.Results;

public interface IResult
{

    List<Error>? Errors { get; }

    bool IsSuccess { get; }

}
public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}