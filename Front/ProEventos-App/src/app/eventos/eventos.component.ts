import { Component, OnInit } from '@angular/core';
import { Evento } from '../models/Evento';
import { EventoService } from '../services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos : Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public mostrarImagem = true;
  private filtroListado = '';

  public get filtroLista(){
    return this.filtroListado
  }
  public set filtroLista(value: string){
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEvetos(this.filtroLista) : this.eventos;
  }
  constructor(private eventoService: EventoService) { }

  public ngOnInit(): void {
    this.getEventos();
  }

  public alterarImagem():void {
    this.mostrarImagem = !this.mostrarImagem;
  }

  public filtrarEvetos(filtrarPor: string):Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }
  public getEventos():void{
    this.eventoService.getEvento().subscribe({
      next: (eventosResp: Evento[]) => {
        this.eventos = eventosResp;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) =>{
        console.log(error)
      }
    });
  }
}
