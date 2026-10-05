using System;
using System.Collections.Generic;
using tpfinal;

namespace tp1
{
	[Serializable]
	public class ArbolGeneral<T>
	{

		private T dato;
		private List<ArbolGeneral<T>> hijos = new List<ArbolGeneral<T>>();

		public ArbolGeneral(T dato)
		{
			this.dato = dato;
		}

		public T getDatoRaiz()
		{
			return this.dato;
		}

		public List<ArbolGeneral<T>> getHijos()
		{
			return hijos;
		}

		public void agregarHijo(ArbolGeneral<T> hijo)
		{
			this.getHijos().Add(hijo);
		}

		public void eliminarHijo(ArbolGeneral<T> hijo)
		{
			this.getHijos().Remove(hijo);
		}


		public bool esHoja()
		{
			return this.getHijos().Count == 0;
		}

		public int altura()
		{
			int alt = -1;
            Cola<ArbolGeneral<T>> cola = new Cola<ArbolGeneral<T>>();
            cola.encolar(this);

            while (!cola.esVacia())
            {
                int n = cola.cantidadElementos();

				for (int i = 0; i < n; i++) 
				{
					var nodo = cola.desencolar();
					foreach (var h in nodo.getHijos()) cola.encolar(h);
				}
				alt++;
            }
            return alt;
		}


		public int nivel(T dato)
		{
            int alt = -1;
            Cola<ArbolGeneral<T>> cola = new Cola<ArbolGeneral<T>>();
            cola.encolar(this);

            while (!cola.esVacia())
            {
				alt++;
                int n = cola.cantidadElementos();

                for (int i = 0; i < n; i++)
                {
                    var nodo = cola.desencolar();
					if (nodo.getDatoRaiz().Equals(dato)) 
					{
						return alt;
					}
                    foreach (var h in nodo.getHijos()) cola.encolar(h);
                }
            }
			return -1;
		}
	}
}
