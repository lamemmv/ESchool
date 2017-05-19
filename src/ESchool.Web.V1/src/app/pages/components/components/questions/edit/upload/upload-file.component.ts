import { Component, OnInit, 
  ViewChild, Renderer, ElementRef, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { NotificationService } from './../../../../../../shared/utils/notification.service';
import { QuestionsService } from './../../questions.service';
import { FormFile } from './../../question.model';
@Component({
  selector: 'q-upload-file',
  styleUrls: [('./upload-file.style.scss')],
  templateUrl: './upload-file.component.html'
})

export class QUploadFileComponent implements OnInit {  

  @ViewChild('fileUpload') public _fileUpload: ElementRef;
  @ViewChild('inputText') public _inputText: ElementRef;
  @Input() defaultValue: string = '';
  private file = new FormFile();
  modalHeader: string;
  modalContent: any;

  constructor(private activeModal: NgbActiveModal,
    private renderer: Renderer,
    private questionService: QuestionsService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {

  }

  bringFileSelector(): boolean {
    this.renderer.invokeElementMethod(this._fileUpload.nativeElement, 'click');
    return false;
  }

  onFileChange(event: any){
    let self = this;
    if (event.target.files && event.target.files.length > 0) {
      let fileToUpload = event.target.files[0];
      self.file.name = fileToUpload.name;
      self.file.type = fileToUpload.type;
      self.file.size = fileToUpload.size;
      this._inputText.nativeElement.value = self.file.name;
    }
  }
  
  save() {
    let fi = this._fileUpload.nativeElement, self = this;
    if (fi.files && fi.files[0]) {
      let fileToUpload = fi.files[0];
      this.questionService
        .upload(fileToUpload)
        .subscribe((response) => {
          self.file.id = response.id;
          self.file.content = response.content;
          self.onUploaded();
        },
        error => {
          self.notificationService.printErrorMessage('Failed to upload file. ' + error);
        });
    }
  }

  onUploaded(): void {
    const self = this;
    const imageElement = String.format('<img alt="{0}" title="{0}" src="{1}" />', 
      self.file.name, 
      self.questionService.getUploadFileUrl() + '/' + self.file.id);
    self.modalContent.insertHtml(imageElement);
    self.activeModal.close(true);
  }

  cancel() {
    this.activeModal.dismiss();
  }

}
