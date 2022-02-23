import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  form!: FormGroup;
  evento = {} as Evento;
  modoSalvar = 'post';
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
  constructor( private fb: FormBuilder, private localeService: BsLocaleService,
    private router: ActivatedRoute, private eventoService: EventoService, private spinner: NgxSpinnerService,
    private toastr: ToastrService ) {
    this.localeService.use('pt-br');
  }

  ngOnInit(): void {
    this.carregaEvento();
    this.validation();
  }

  public carregaEvento():void{
    const eventoIdParam = this.router.snapshot.paramMap.get('id');
    if(eventoIdParam != null){
      this.spinner.show();
      this.modoSalvar = 'put';
      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = {... evento};
          this.form.patchValue(this.evento);
        },
        error: (error:any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar evento.', 'Erro!');
          console.error(error);
        },
        complete: () => {
          this.spinner.hide()
        }
      });
    }
  }

  public validation():void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: ['', Validators.required]
    });
  }
  public limpaForm():void{
    this.form.reset();
  }
  public cssValidator(valor: FormControl):any{
    return {'is-invalid':valor.errors && valor.touched}
  }

  public salvarAlteracao():void{
    this.spinner.show();
    if(this.form.valid){
      this.evento = (this.modoSalvar == 'post')?{...this.form.value}:this.evento =
      {id:this.evento.id, ...this.form.value};
      this.eventoService[this.modoSalvar](this.evento).subscribe(
        () => this.toastr.success('Evento salvo com sucesso!', 'Sucesso'),
        (error:any) => {
          console.error(error);
          this.toastr.error('Erro ao salvar evento', 'Erro');
        },
      ).add(() => this.spinner.hide());
    }
  }
}
