export function attachCpfMaskEventHandler(element) {
	element.addEventListener('keydown', function (e) {
		const navigationKeys = ['ArrowLeft', 'ArrowRight', 'Tab', 'Home', 'End'];

		if (navigationKeys.includes(e.key)) {
			return;
		}

		if (e.key == 'Backspace') {
			if (element.selectionStart != element.selectionEnd) {
				return;
			}

			const cursor = element.selectionStart;
			const cursorPos = cursor - 1;
			let value = element.value;

			if (cursor != 0 && cursor % 4 == 0) {
				value = value.slice(0, cursorPos) + value.slice(cursor)
				element.value = value;
				element.setSelectionRange(cursorPos, cursorPos);
			}

			return;
		}

		if (e.key == 'Delete') {
			if (element.selectionStart != element.selectionEnd) {
				return;
			}

			const cursor = element.selectionStart;
			let value = element.value;

			if ((cursor + 1) % 4 == 0) {
				value = value.slice(0, cursor) + value.slice(cursor + 1)
				element.value = value;
				element.setSelectionRange(cursor, cursor);
			}

			return;
		}

		if ((e.code < 'Digit0' || e.code > 'Digit9') && (e.code < 'Numpad0' || e.code > 'Numpad9')) {
			e.preventDefault();
		}
	});

	element.addEventListener('input', function (e) {
		let cursor = element.selectionStart;
		const oldValue = element.value;

		// Remove non-digit characters
		let value = element.value.replace(/\D/g, '')

		// Format the value
		if (value.length > 3 && value.length < 7) {
			value = value.replace(/(\d{3})(\d)/, '$1.$2');
		} else if (value.length > 6 && value.length < 10) {
			value = value.replace(/(\d{3})(\d{3})(\d)/, '$1.$2.$3');
		} else {
			value = value.replace(/(\d{3})(\d{3})(\d{3})(\d)/, '$1.$2.$3-$4');
		}

		element.value = value;

		if (e.inputType != 'deleteContentBackward' && e.inputType != 'deleteContentForward') {
			cursor += (value.length - oldValue.length);

			if (value.length - oldValue.length == 0) {
				cursor += 1;
			}
		}

		element.setSelectionRange(cursor, cursor);
	})
};