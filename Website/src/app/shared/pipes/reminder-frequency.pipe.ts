import { Pipe, PipeTransform } from '@angular/core';
import { reminderFrequencies } from '@core/domain-classes/reminder-frequency';
import { TranslationService } from '@core/services/translation.service';

@Pipe({
  name: 'frequency',
})
export class ReminderFrequencyPipe implements PipeTransform {
  transform(value: any, ...args: any[]): any {
    const reminderFrequency = reminderFrequencies.find((c) => c.id == value);
    if (reminderFrequency) {
      return reminderFrequency.name.toUpperCase();
    }
    return '';
  }
}
