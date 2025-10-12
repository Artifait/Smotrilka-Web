class AddLinkManager {
    constructor() {
        this.stickers = [];
        this.init();
    }

    init() {
        this.bindEvents();
        this.switchStickerType('author');
    }

    bindEvents() {
        // Переключение типа стикера
        const stickerTypeSelect = document.getElementById('stickerType');
        if (stickerTypeSelect) {
            stickerTypeSelect.addEventListener('change', (e) => {
                this.switchStickerType(e.target.value);
            });
        }

        // Добавление стикера
        const addStickerBtn = document.getElementById('addSticker');
        if (addStickerBtn) {
            addStickerBtn.addEventListener('click', () => {
                this.addSticker();
            });
        }

        // Обработка отправки формы
        const form = document.getElementById('addLinkForm');
        if (form) {
            form.addEventListener('submit', (e) => {
                this.handleFormSubmit(e);
            });
        }
    }

    switchStickerType(type) {
        // Скрываем все поля стикеров
        document.querySelectorAll('.sticker-fields').forEach(field => {
            field.style.display = 'none';
        });

        // Показываем нужные поля
        const targetFields = document.getElementById(`${type}Sticker`);
        if (targetFields) {
            targetFields.style.display = 'block';
        }
    }

    addSticker() {
        const type = document.getElementById('stickerType').value;
        let stickerData = {};

        try {
            switch (type) {
                case 'author':
                    const author = document.getElementById('author').value.trim();
                    if (!author) {
                        alert('Введите имя автора');
                        return;
                    }
                    stickerData = {
                        key: 'author',
                        value: JSON.stringify({ author: author })
                    };
                    break;

                case 'language':
                    const language = document.getElementById('language').value.trim();
                    const isMachineTranslated = document.getElementById('isMachineTranslated').checked;
                    const originalLanguage = document.getElementById('originalLanguage').value.trim();
                    const exactDate = document.getElementById('exactDate').value;

                    if (!language) {
                        alert('Введите язык');
                        return;
                    }

                    if (isMachineTranslated && !originalLanguage) {
                        alert('Введите оригинальный язык для машинного перевода');
                        return;
                    }

                    stickerData = {
                        key: 'language',
                        value: JSON.stringify({
                            language: language,
                            isMachineTranslated: isMachineTranslated,
                            originalLanguage: originalLanguage,
                            exactDate: exactDate || null
                        })
                    };
                    break;
            }

            // Добавляем стикер в список
            this.stickers.push(stickerData);
            this.updateStickersList();
            this.clearStickerForm();

        } catch (error) {
            console.error('Error adding sticker:', error);
            alert('Ошибка при добавлении стикера');
        }
    }

    updateStickersList() {
        const stickersList = document.getElementById('stickersList');
        const stickersJson = document.getElementById('stickersJson');

        if (!stickersList) return;

        // Обновляем JSON
        stickersJson.value = JSON.stringify(this.stickers);

        // Обновляем визуальный список
        stickersList.innerHTML = this.stickers.map((sticker, index) => {
            let displayText = '';
            try {
                const parsedValue = JSON.parse(sticker.value);
                if (sticker.key === 'author') {
                    displayText = `Автор: ${parsedValue.author}`;
                } else if (sticker.key === 'language') {
                    displayText = parsedValue.isMachineTranslated
                        ? `Язык: ${parsedValue.language} (машинный перевод с ${parsedValue.originalLanguage})`
                        : `Язык: ${parsedValue.language}`;

                    if (parsedValue.exactDate) {
                        displayText += ` - ${new Date(parsedValue.exactDate).toLocaleDateString('ru-RU')}`;
                    }
                }
            } catch {
                displayText = `${sticker.key}: ${sticker.value}`;
            }

            return `
                <div class="sticker-item" data-index="${index}">
                    <span class="sticker-display">${displayText}</span>
                    <button type="button" class="btn-remove-sticker" onclick="addLinkManager.removeSticker(${index})">×</button>
                </div>
            `;
        }).join('');
    }

    removeSticker(index) {
        this.stickers.splice(index, 1);
        this.updateStickersList();
    }

    clearStickerForm() {
        document.getElementById('author').value = '';
        document.getElementById('language').value = '';
        document.getElementById('isMachineTranslated').checked = false;
        document.getElementById('originalLanguage').value = '';
        document.getElementById('exactDate').value = '';
    }

    handleFormSubmit(e) {
        // Валидация формы
        const name = document.getElementById('name').value.trim();
        const link = document.getElementById('link').value.trim();

        if (!name) {
            e.preventDefault();
            this.showError('nameError', 'Введите название ссылки');
            return;
        }

        if (!link) {
            e.preventDefault();
            this.showError('linkError', 'Введите ссылку');
            return;
        }

        // Проверяем URL
        try {
            new URL(link);
        } catch {
            e.preventDefault();
            this.showError('linkError', 'Введите корректный URL');
            return;
        }

        // Очищаем ошибки
        this.clearErrors();
    }

    showError(fieldId, message) {
        const errorElement = document.getElementById(fieldId);
        if (errorElement) {
            errorElement.textContent = message;
        }
    }

    clearErrors() {
        document.querySelectorAll('.error-message').forEach(element => {
            element.textContent = '';
        });
    }
}

// Глобальная переменная для доступа из HTML
let addLinkManager;

document.addEventListener('DOMContentLoaded', () => {
    addLinkManager = new AddLinkManager();
});