import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder,
  FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { environment } from '@environments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef
  eventoId: number;
  form!: FormGroup;
  evento = {} as Evento;
  modoSalvar = 'post';
  loteAtual = {id: 0, nome: '', indice: 0};
  imagemURL = 'assets/img/upload.png';
  file: File;

  get modoEditar(): boolean {
    return this.modoSalvar == 'put';
  }
  get lotes():FormArray{
    return this.form.get('lotes') as FormArray;
  }
  get f():any{
    return this.form.controls;
  }
  get bsConfig():any{
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }
  get bsConfigLote():any{
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }
  constructor( private fb: FormBuilder, private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute, private eventoService: EventoService, private spinner: NgxSpinnerService,
    private toastr: ToastrService, private router: Router, private loteService: LoteService,
    private modalServie: BsModalService) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.carregaEvento();
    this.validation();
  }

  public carregaEvento():void{
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');
    if(this.eventoId != null && this.eventoId != 0){
      this.spinner.show();
      this.modoSalvar = 'put';
      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {... evento};
          this.form.patchValue(this.evento);
          if(this.evento.imagemURL != ''){
            this.imagemURL = environment.apiURL + 'resources/images/' + this.evento.imagemURL;
          }
          //this.evento.lotes.forEach(lote =>{ this.lotes.push(this.criaLote(lote))});
           this.carregarLotes();
        },
        (error:any) => {
          this.toastr.error('Erro ao carregar evento.', 'Erro!');
          console.error(error);
        },
      ).add(()=>this.spinner.hide());
    }
  }

  public carregarLotes():void{
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote =>{ this.lotes.push(this.criaLote(lote))});
      },
      (error:any) => {
        this.toastr.error('Erro ao carregar lotes', 'Erro');
        console.error(error);
      }
    ).add(()=>this.spinner.hide())
  }

  public validation():void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: [''],
      lotes: this.fb.array([]),
    });
  }
  public adicionaLote():void{
    this.lotes.push(this.criaLote({id:0} as Lote));
  }
  public criaLote(lote:Lote):FormGroup{
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    });
  }
  public limpaForm():void{
    this.form.reset();
  }
  public cssValidator(valor: FormControl | AbstractControl):any{
    return {'is-invalid': valor.errors && valor.touched};
  }

  public salvarEvento():void{
    this.spinner.show();
    if(this.form.valid){
      this.evento = (this.modoSalvar == 'post')?{...this.form.value}:this.evento =
      {id:this.evento.id, ...this.form.value};
      this.eventoService[this.modoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com sucesso!', 'Sucesso');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
        },
        (error:any) => {
          console.error(error);
          this.toastr.error('Erro ao salvar evento', 'Erro');
        },
      ).add(() => this.spinner.hide());
    }
  }
  public salvarLote():void{
    if(this.form.controls.lotes.valid){
      this.spinner.show();
      this.loteService.saveLote(this.eventoId, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!');
          this.lotes.reset();
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar salvar lotes.', 'Erro');
          console.error(error);
        }
      ).add(()=>this.spinner.hide());
    }
  }
  public removerLote(template: TemplateRef<any>, indice: number): void{
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;
    this.modalRef = this.modalServie.show(template, {class: 'modal-sm'});

  }
  public confirmDeleteLote():void{
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () =>{
        this.toastr.success('Lote deletado com sucesso!', 'Sucesso');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error:any) =>{
        this.toastr.error(`Erro ao deletar lote ${this.loteAtual.id}`, 'Erro');
        console.error(error);
      }
    ).add(()=>this.spinner.hide());
  }
  public declineDeleteLote():void{
    this.modalRef.hide();
  }
  public onFileChange(ev: any):void{
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;
    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);
    this.uploadImage();
  }
  public uploadImage():void{
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregaEvento();
        this.toastr.success('Imagem atualizada com Sucesso', 'Sucesso');
      },
      (error: any) => {
        this.toastr.error('Erro ao fazer upload de imagem', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }
  // public mudaData(value: Date, indice: number, campo: string):void{
  //   this.lotes.value[indice][campo] = value;
  // } Solução dada pelo professor para corrigir o invalid date, porém tive problemas e decidi tomar uma
  //solução alternativa
}
