import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, MaxValidator, RequiredValidator, Validators } from '@angular/forms';
import { ValidatorPass } from '@app/helpers/ValidatorPass';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {
  public form!: FormGroup;
  public get b():any{
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
      pNome: ['', [Validators.required, Validators.minLength(3)]],
      uNome: ['', [Validators.required, Validators.minLength(4)]],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      senha: ['', [Validators.required, Validators.minLength(4)]],
      cSenha: ['', Validators.required]
    }, formOptions);
  }

}
