using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IDirectorService
	{
		void AddDirector(Director director);
		Director GetDirectorById(int id);
		List<Director> GetDirectorList();
		void RemoveDirector(Director director);
		void UpdateDirector(Director director);
	}
}
