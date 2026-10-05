
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace tpfinal
{

	public class Estrategia
	{
		
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            if (arbol == null) return "Árbol no disponible.";
            var resultado = dfsURLPorId(arbol, "", id);

            if (resultado != null) return resultado;
            return "Id no encontrado.";
        }

        private string? dfsURLPorId(ArbolGeneral<ItemCat> nodo, string camino, int id) 
        {
            camino += nodo.getDatoRaiz().Nombre + '/';
            if (nodo.getDatoRaiz().Id == id) return camino;

            foreach (var hijo in nodo.getHijos()) 
            {
                var respuesta = dfsURLPorId(hijo, camino, id);
                if (respuesta != null) return respuesta;
            }
            return null;
        }

        private void dfsURL(ArbolGeneral<ItemCat> nodo, string camino, List<string> resultado) 
        {
            camino = camino + nodo.getDatoRaiz().Nombre + '/';
            bool tieneCategoriaHija = false;

            foreach (var hijo in nodo.getHijos()) 
            {
                if (hijo.getDatoRaiz().Tipo == TipoElemento.Categoria)
                {
                    tieneCategoriaHija = true;
                    dfsURL(hijo, camino, resultado);
                }
            }

            if (!tieneCategoriaHija) resultado.Add(camino);
        }

        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
		{
            List<string> resultado = new List<string>();
            if (arbol == null) return resultado;
           
            dfsURL(arbol, "", resultado);
			return resultado;
		}
        

              

        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
		{
            var resultado = new List<List<string>>();
            if (arbol == null) return resultado;

            Cola<ArbolGeneral<ItemCat>> cola = new Cola<ArbolGeneral<ItemCat>>();
            cola.encolar(arbol);

            while (!cola.esVacia()) 
            {
                int n = cola.cantidadElementos();
                var nivelActual = new List<string>();

                for (int i = 0; i < n; i++)
                {
                    var nodo = cola.desencolar();
                    nivelActual.Add(nodo.getDatoRaiz().Nombre);
                    
                    foreach (var v in nodo.getHijos()) cola.encolar(v);
                }

                resultado.Add(nivelActual);
            }

            return resultado;
        }


        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> lista = new List<ItemCat>();
            if (arbol == null) return lista;

            lista.Add(arbol.getDatoRaiz());

            foreach (var hijos in arbol.getHijos())
            {
                lista.AddRange(Todos(hijos));
            }
            return lista;
        }

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
		{
            string[] ruta = rutaAlPadre.Split('/');
            var actual = arbol;
            bool vacio = false;
            foreach (var s in ruta) 
            {
                if (s == "") 
                {
                    vacio = true;
                }
            }

            if (ruta.Length == 0 || vacio) 
            {
                throw new ArgumentException("La ruta es inválida o está vacía.");
            }

            for (int i = 0; i < ruta.Length; i++) 
            {
                bool encontrado = false;

                foreach (var s in actual.getHijos()) 
                {
                    if (s.getDatoRaiz().Nombre == ruta[i]) 
                    {
                        actual = s;
                        encontrado = true;
                    }
                }

                if (!encontrado) 
                {
                    ItemCat nuevoItem = new ItemCat(ruta[i], TipoElemento.Categoria);
                    ArbolGeneral<ItemCat> nuevoArbol = new ArbolGeneral<ItemCat>(nuevoItem);
                    actual.agregarHijo(nuevoArbol);
                }
            }
            ArbolGeneral<ItemCat> n = new ArbolGeneral<ItemCat>(dato);
            actual.agregarHijo(n);
        }

        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
            List<ItemCat> listita = new List<ItemCat>();
            if (string.IsNullOrWhiteSpace(elementoABuscar)) return listita;
            dfsBuscarRecursivo(arbol, listita, elementoABuscar);
            return listita;
		}
        private void dfsBuscarRecursivo(ArbolGeneral<ItemCat> nodo, List<ItemCat> lista, string comparar) 
        {
            var actual = nodo.getDatoRaiz();

            if (Coincide(actual.Nombre, comparar)) lista.Add(nodo.getDatoRaiz());

            foreach (var hijo in nodo.getHijos())
            {
                dfsBuscarRecursivo(hijo, lista, comparar);
            }
        }
        private bool Coincide(string n, string t) 
        {
            if (n == null) return false;
            return comparador.IndexOf(n, t, OPCIONES) >= 0;
        }
        private static readonly CompareInfo comparador = CultureInfo.InvariantCulture.CompareInfo; 
        private const CompareOptions OPCIONES = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;
    }
}