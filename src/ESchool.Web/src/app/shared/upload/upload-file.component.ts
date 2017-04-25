import { Component } from '@angular/core';
import { FileUploader, FileUploaderOptions } from 'ng2-file-upload';

@Component({
    selector: 'eschool-upload',
    templateUrl: './upload-file.component.html'
})
export class UploadFileComponent {
    public uploader: FileUploader = new FileUploader({ url: 'http://localhost:27629/admin/files' });
    private options: FileUploaderOptions;
    constructor() {
        let self = this;
        self.options = {};
        self.options.method = 'POST';
        self.options.autoUpload = false;
        this.uploader.setOptions(self.options);
    }
}
