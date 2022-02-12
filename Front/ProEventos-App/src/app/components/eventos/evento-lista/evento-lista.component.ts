import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
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
  constructor(private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) { }

  public ngOnInit(): void {
    this.spinner.show();
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
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos','Erro');
      },
      complete:() => this.spinner.hide()
    });
  }
  public openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.toastr.success('O evento foi deletado.','Sucesso!');
  }

  public decline(): void {
    this.modalRef?.hide();
    this.toastr.success('Talvez mais tarde...','Ok!');
  }
  public detalheEvento(id: number):void{
    this.router.navigate([`eventos/detalhe/${id}`])
  }
}
