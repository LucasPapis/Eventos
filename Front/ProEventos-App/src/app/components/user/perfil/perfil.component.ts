import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorPass } from '@app/helpers/ValidatorPass';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  public form!: FormGroup;
  public get p():any{
    return this.form.controls;
  }
  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validation();
  }
  public validation():void{
    const formOptions: AbstractControlOptions = {
      validators: ValidatorPass.ValidaSenha('senha', 'cSenha')
    };
    this.form = this.fb.group({
      titulo: ['', Validators.required],
      pNome: ['', [Validators.required, Validators.minLength(3)]],
      uNome: ['', [Validators.required, Validators.minLength(4)]],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', Validators.required],
      funcao: ['', Validators.required],
      descricao: ['', Validators.required],
      senha: ['', [Validators.required, Validators.minLength(4)]],
      cSenha: ['', Validators.required]
    }, formOptions);
  }
  public limpaForm(event: any):void{
    event.preventDefault();
    this.form.reset();
  }
}
