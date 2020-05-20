class HtmlEditor {
    constructor(TextArea, ToolsContainer) {
        this.MainDiv = TextArea;
        this.ToolsDiv = ToolsContainer;

        var test = document.createElement("div");
        test.className = "HtmlEditorToolsWrapper";

        test.innerHTML += `
                            <style>
                                .HtmlEditorToolsWrapper {
                                    position: relative;
                                    width: 100%;
                                    margin-top: 10px;
                                    margin-bottom: 10px;
                                }

                                .HtmlEditorToolsWrapper button {
                                    border: none;
                                    margin-right: 5px;
                                    background-color: white;
                                    border-radius: 10px;
                                    font-size: x-large;
                                    height: 40px;
                                    padding: 0;
                                    padding-left: 5px;
                                    padding-right: 5px;
                                    min-width: 25px;
                                }

                                .HtmlEditorToolsWrapper button:hover {
                                    background-color: #cddc39;
                                }
                                
                                .HtmlEditorToolsWrapper button img {
                                    height: 30px;
                                }
                            </style>
                            `;
        test.innerHTML += "<button type='button'><img src='https://image.flaticon.com/icons/png/512/23/23765.png' /></button>"
        test.innerHTML += "<button type='button'><i>i</i></button>";
        this.ToolsDiv.appendChild(test);
    }
}