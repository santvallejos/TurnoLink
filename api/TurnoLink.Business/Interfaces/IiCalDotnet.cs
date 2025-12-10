using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnoLink.Business.DTOs;

namespace TurnoLink.Business.Interfaces
{
    /// <summary>
    /// Interface for iCal.NET related operations.
    /// </summary>
    public interface IiCalDotnet
    {
        /// <summary>
        /// Creates a new .ics file for a booking
        /// </summary>
        /// <param name="id">Booking</param>
        Task<string> CreateFileIcsBookingAsync(BookingDto booking);
    }
}
