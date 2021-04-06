using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjProjUnitTest.Api.Controllers;
using ProjUnitTest.Api.Models;
using Xunit;

namespace ProjUnitTest.Tests
{
	public class PendenciaUnitTest
	{
		private DbContextOptions<PendenciaContext> options;

		private void InitializeDataBase()
		{

			//create a Temporary Database
			options = new DbContextOptionsBuilder<PendenciaContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			   .Options;

			// Insert data into the database using one instance of the context
			using (var context = new PendenciaContext(options))
			{
				context.Pendencia.Add(new Pendencia { Id = 1, Descricao = "Aberto", DataCadastro = DateTime.Now });
				context.Pendencia.Add(new Pendencia { Id = 2, Descricao = "Aberto", DataCadastro = DateTime.Now });
				context.Pendencia.Add(new Pendencia { Id = 3, Descricao = "Aberto", DataCadastro = DateTime.Now });
				context.SaveChanges();
			}
		}
		[Fact]
		public void GetAll()
		{
			InitializeDataBase();

			//Use a clean instance of the context to run the test
			using (var context = new PendenciaContext(options))
			{
				PendenciaController pendenciaController = new PendenciaController(context);
				IEnumerable<Pendencia> p = pendenciaController.GetPendencia().Result.Value;
				Assert.Equal(3, p.Count());
				
			}
		}

		[Fact]
		public void GetbyId()
		{
			InitializeDataBase();

			// Use a clean instance of the context to run the test
			using (var context = new PendenciaContext(options))
			{
				int pendenciaId = 2;
				PendenciaController pendenciaController = new PendenciaController(context);
				Pendencia pendencia = pendenciaController.GetPendencia(pendenciaId).Result.Value;
				Assert.Equal(2, pendencia.Id);
			}
		}

		[Fact]
		public void Create()
		{
			InitializeDataBase();

			Pendencia pendencia = new Pendencia()
			{
				Id = 4,
				Descricao = "Bolsa",
				DataCadastro = DateTime.Now
			};

			//Use a clean instance of the context to run the test
			using (var context = new PendenciaContext(options))
			{
				PendenciaController pendenciaController = new PendenciaController(context);
				Pendencia pendencia1 = pendenciaController.PostPendencia(pendencia).Result.Value;
				Assert.Equal(4, pendencia1.Id);
			}
		}

		[Fact]
		public void Update()
		{
			InitializeDataBase();
			Pendencia pendencia = new Pendencia()
			{
				Id = 2,
				Descricao = "Sapato",
				DataCadastro = DateTime.Now
			};

			//Use a clean instance of the context to run the test
			using (var context = new PendenciaContext(options))
			{
				PendenciaController pendenciaController = new PendenciaController(context);
				Pendencia pendencia1 = pendenciaController.PutPendencia(2, pendencia).Result.Value;
				Assert.Equal("Sapato", pendencia1.Descricao);
			}
		}

		[Fact]
		public void Delete()
		{
			InitializeDataBase();
			using (var context = new PendenciaContext(options))
			{
				PendenciaController pendenciaController = new PendenciaController(context);
				Pendencia pendencia = pendenciaController.DeletePendencia(2).Result.Value;
				Assert.Null(pendencia);
			}
		}
	}
}
