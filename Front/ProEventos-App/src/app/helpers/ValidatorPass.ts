import { AbstractControl, FormControlName, FormGroup } from '@angular/forms';

export class ValidatorPass {
  static ValidaSenha(controlName: string, confirmControlName: string): any{
    return (group: AbstractControl) => {
      const formGroup = group as FormGroup;
      const control = formGroup.controls[controlName];
      const confirmControl = formGroup.controls[confirmControlName];
      if(confirmControl.errors && !confirmControl.errors.mustMatch){
        return null
      }
      if(control.value != confirmControl.value){
        confirmControl.setErrors({mustMatch: true});
      }else{
        confirmControl.setErrors(null);
      }
      return null;
    }
  }
}
