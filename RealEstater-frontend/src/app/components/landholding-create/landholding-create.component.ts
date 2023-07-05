import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { switchMap } from 'rxjs';
import Common from 'src/app/helpers/Common';
import LandholdingMapper from 'src/app/helpers/LandholdingMapper';
import ValidateForm from 'src/app/helpers/ValidateForm';
import { AuthService } from 'src/app/services/auth.service';
import { CitiesService } from 'src/app/services/cities.service';
import { LandholdingService } from 'src/app/services/landholding.service';

@Component({
  selector: 'app-landholding-create',
  templateUrl: './landholding-create.component.html',
  styleUrls: ['./landholding-create.component.css']
})
export class LandholdingCreateComponent implements OnInit {

  cities: any = [];
  constructionStages: any = [];
  constructionTypes: any = [];
  landholdingTypes: any = [];
  featuresDropDownList: any = [];
  featuresForLandholding: any = [];
  landholdingForm!: FormGroup;
  pictures: any = [];

  constructor(private fb: FormBuilder,
    private cityService: CitiesService,
    private toast: NgToastService,
    private landholdingService: LandholdingService,
    private auth: AuthService,
    private router: Router) { }

  ngOnInit(): void {

    this.landholdingForm = this.fb.group({
      title: new FormControl('', [Validators.required]),
      yearOfConstruction: new FormControl('', [Validators.required, Validators.pattern(Common.regexForYear)]),
      city: new FormControl('', [Validators.required]),
      address: new FormControl('', [Validators.required, Validators.pattern(Common.regexForAddress)]),
      materialType: new FormControl('', [Validators.required]),
      type: new FormControl('', [Validators.required]),
      stage: new FormControl('', [Validators.required]),
      numberOfFloors: new FormControl('', [Validators.min(0)]),
      floorOfResidence: new FormControl('', [Validators.min(0)]),
      area: new FormControl('', [Validators.required, Validators.min(0)]),
      courtyard: new FormControl('', [Validators.min(0)]),
      price: new FormControl('', [Validators.required, Validators.min(0)]),
      description: new FormControl('', [Validators.required, Validators.minLength(15), Validators.maxLength(500)]),
    })

    this.cityService.getAllCities().pipe(
      switchMap((cities) => {
        this.cities = cities;
        return this.landholdingService.getAllConstructionStages();
      }),
      switchMap((constructionStages) => {
        this.constructionStages = constructionStages;
        return this.landholdingService.getAllConstructionTypes();
      }),
      switchMap((constructionTypes) => {
        this.constructionTypes = constructionTypes;
        return this.landholdingService.getAllLandholdingTypes();
      }),
      switchMap((landholdingTypes) => {
        this.landholdingTypes = landholdingTypes;
        return this.landholdingService.getAllFeatures();
      })
    )
    .subscribe(features => {
      this.featuresDropDownList = features;
    });
  }

  addToFeatures() {
    let selectElement = document.getElementById("feature") as HTMLSelectElement;
    let selectedOptionIndex = selectElement.selectedIndex;
    let selectedOption = selectElement.options[selectedOptionIndex];
    let selectedOptionValue = selectedOption.value;

    if (this.featuresForLandholding.includes(selectedOptionValue)) {
      this.toast.warning({ summary: "Feature is already in!", duration: 5000 });
    }
    else {
      this.featuresForLandholding.push(selectedOptionValue);
    }
  }

  removeFromFeatures(feature: string) {
    let index = this.featuresForLandholding.findIndex((d: string) => d === feature);
    this.featuresForLandholding.splice(index, 1);
  }

  onSubmit() {
    if (this.landholdingForm.valid) {
      const tokenPayload = this.auth.decodeToken();
      const email = tokenPayload.unique_name;
      let newLandholing = LandholdingMapper.createLandholding(this.landholdingForm, this.featuresForLandholding, this.pictures);
      this.landholdingService.createNewLandholding(newLandholing, email)
        .subscribe({
          next: (res) => {
            this.toast.success({ summary: "Landholding created successfully!", duration: 5000 });
            this.router.navigate(["/user-landholdings"]);
          },
          error: (err) => {
            this.toast.error({ summary: err.error.message, duration: 5000 });
          }
        })
    }
    else {
      ValidateForm.validateAllFormFields(this.landholdingForm);
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
        this.pictures.push(reader.result);
      }
      reader.readAsDataURL(file);
    }
  }

  removePictures() {
    this.pictures = [];
    (<HTMLInputElement>document.getElementById('uploadPicture')).value = "";
  }
}
