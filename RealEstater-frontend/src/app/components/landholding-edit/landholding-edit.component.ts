import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { switchMap } from 'rxjs';
import Common from 'src/app/helpers/Common';
import LandholdingMapper from 'src/app/helpers/LandholdingMapper';
import ValidateForm from 'src/app/helpers/ValidateForm';
import { AuthService } from 'src/app/services/auth.service';
import { CitiesService } from 'src/app/services/cities.service';
import { LandholdingService } from 'src/app/services/landholding.service';

@Component({
  selector: 'app-landholding-edit',
  templateUrl: './landholding-edit.component.html',
  styleUrls: ['./landholding-edit.component.css']
})
export class LandholdingEditComponent implements OnInit {
  landholding: any;
  landholdingId!: number;
  editLandholdingForm!: FormGroup;
  isActive: boolean = true;

  cities: any = [];
  constructionStages: any = [];
  constructionTypes: any = [];
  landholdingTypes: any = [];
  featuresDropDownList: any = [];

  constructor(private route: ActivatedRoute,
    private fb: FormBuilder,
    private cityService: CitiesService,
    private toast: NgToastService,
    private landholdingService: LandholdingService,
    private auth: AuthService,
    private router: Router) { }

  ngOnInit(): void {
    this.landholdingId = Number(this.route.snapshot.paramMap.get('id'))!;

    this.editLandholdingForm = this.fb.group({
      title: new FormControl('', [Validators.required]),
      yearOfConstruction: new FormControl('', [Validators.required, Validators.pattern(Common.regexForYear)]),
      address: new FormControl('', [Validators.required, Validators.pattern(Common.regexForAddress)]),
      city: new FormControl('', [Validators.required]),
      materialType: new FormControl('', [Validators.required]),
      type: new FormControl('', [Validators.required]),
      stage: new FormControl('', [Validators.required]),
      numberOfFloors: new FormControl('', [Validators.required, Validators.min(0)]),
      floorOfResidence: new FormControl('', [Validators.required, Validators.min(0)]),
      area: new FormControl('', [Validators.required, Validators.min(0)]),
      courtyard: new FormControl('', [Validators.required, Validators.min(0)]),
      price: new FormControl('', [Validators.required, Validators.min(0)]),
      description: new FormControl('', [Validators.required, Validators.minLength(15), Validators.maxLength(500)]),
    });

    this.landholdingService.getLandholdingById(this.landholdingId).pipe(
      switchMap((res) => {
        this.landholding = res;
        this.isActive = res.isActive;

        this.editLandholdingForm.patchValue({
          title: this.landholding.title,
          yearOfConstruction: this.landholding.yearOfConstruction,
          materialType: this.landholding.constructionMaterial,
          type: this.landholding.landholdingType,
          stage: this.landholding.constructionStage,
          address: this.landholding.address,
          numberOfFloors: this.landholding.numberOfFloors,
          floorOfResidence: this.landholding.floor,
          area: this.landholding.area,
          courtyard: this.landholding.courtyard,
          price: this.landholding.price,
          description: this.landholding.description,
          city: this.landholding.city
        });

        return this.cityService.getAllCities();
      }),
      switchMap((res) => {
        this.cities = res;
        return this.landholdingService.getAllConstructionStages();
      }),
      switchMap((res) => {
        this.constructionStages = res;
        return this.landholdingService.getAllConstructionTypes();
      }),
      switchMap((res) => {
        this.constructionTypes = res;
        return this.landholdingService.getAllLandholdingTypes();
      }),
      switchMap((res) => {
        this.landholdingTypes = res;
        return this.landholdingService.getAllFeatures();
      })
    ).subscribe((res) => {
      this.featuresDropDownList = res;

      //remove spinners and show content
      (document.getElementById('spinner') as HTMLElement).style.display = "none";
      (document.getElementById('contentOfEdit1') as HTMLElement).style.display = "inline";
      (document.getElementById('contentOfEdit2') as HTMLElement).style.display = "inline";
      (document.getElementById('contentOfEdit3') as HTMLElement).style.display = "inline";

      this.editLandholdingForm.controls['city'].markAsTouched();
      this.editLandholdingForm.controls['city'].updateValueAndValidity();

      this.editLandholdingForm.controls['materialType'].markAsTouched();
      this.editLandholdingForm.controls['materialType'].updateValueAndValidity();

      this.editLandholdingForm.controls['type'].markAsTouched();
      this.editLandholdingForm.controls['type'].updateValueAndValidity();

      this.editLandholdingForm.controls['stage'].markAsTouched();
      this.editLandholdingForm.controls['stage'].updateValueAndValidity();
    });

  }
  
  addToFeatures() {
    let selectElement = document.getElementById("feature") as HTMLSelectElement;
    let selectedOptionIndex = selectElement.selectedIndex;
    let selectedOption = selectElement.options[selectedOptionIndex];
    let selectedOptionValue = selectedOption.value;

    if (this.landholding.features.includes(selectedOptionValue)) {
      this.toast.warning({ summary: "Feature is already in!", duration: 5000 });
    }
    else {
      this.landholding.features.push(selectedOptionValue);
    }
  }
  removeFromFeatures(feature: string) {
    let index = this.landholding.features.findIndex((d: string) => d === feature);
    this.landholding.features.splice(index, 1);
  }

  onUpdate() {
    if (this.editLandholdingForm.valid) {
      let newLandholing = LandholdingMapper.createLandholding(this.editLandholdingForm, this.landholding.features, this.landholding.pictures, this.landholdingId, this.isActive);
      let email = this.auth.decodeToken().unique_name;
      this.landholdingService.updateLandholding(newLandholing, email)
        .subscribe({
          next: () => {
            this.toast.success({ summary: "Landholding updated successfully!", duration: 5000 });
            this.router.navigate(["/user-landholdings"]);
          },
          error: (err) => {
            this.toast.error({ summary: err.error.message, duration: 5000 });
          }
        })
    }
    else {
      ValidateForm.validateAllFormFields(this.editLandholdingForm);
      this.toast.error({ summary: "Please make sure all the fields are entered!", duration: 5000 });
    }
  }

  selectFile(event: any) {
    if (!event.target.files[0] || event.target.files[0].length == 0) {
      (<HTMLInputElement>document.getElementById('uploadPicture')).value = "";
      this.toast.error({ summary: "You must select an image!", duration: 5000 });
      return;
    }

    var mimeType = event.target.files[0].type;

    if (mimeType.match(/image\/*/) == null) {
      (<HTMLInputElement>document.getElementById('uploadPicture')).value = "";
      this.toast.error({ summary: "Only images are supported", duration: 5000 });
      return;
    }

    let files = event.target.files;

    let file;
    for (let i = 0; i < files.length; i++) {
      let reader = new FileReader();
      file = files[i];
      reader.onload = (file) => {
        this.landholding.pictures.push(reader.result);
      }
      reader.readAsDataURL(file)
    }
  }

  removePictures() {
    this.landholding.pictures = [];
    (<HTMLInputElement>document.getElementById('uploadPicture')).value = "";
  }

  onToggleChecked() {
    this.isActive = !this.isActive;
  }

  deleteLandholding() {
    this.landholdingService.deleteLandholding(this.landholdingId).subscribe({
      next: () => {
        this.toast.success({ summary: "Landholding deleted successfully!", duration: 5000 });
        this.router.navigate(["/user-landholdings"]);
      },
      error: (err) => {
        this.toast.error({ summary: err.error.message, duration: 5000 });
      }
    })
  }
}
