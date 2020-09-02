class MovementBox extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: [] };
        this.handleMovementSubmit = this.handleMovementSubmit.bind(this);
        this.loadMovementsFromServer = this.loadMovementsFromServer.bind(this);
    }

    loadMovementsFromServer(movement) {
        const xhr = new XMLHttpRequest();
        // "/movement is not real url" why? this does not work
        //let url = new URL(this.props.url);
        var url = '';
        if (movement) {
            //url.searchParams.set('name', movement.name);
            url = this.props.url + "?name=" + movement.name;
        } else {
            url = this.props.url;
        }
        console.log(url);
        xhr.open('get', url, true);
        xhr.onload = () => {
            console.log("onload done.");

            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
    }

    handleMovementSubmit(movement) {
        console.log('handleMovementSubmit');

        const data = new FormData();
        data.append('Name', movement.name);
        data.append('Type', movement.type);
        console.log(movement.name);
        console.log(movement.type);

        const xhr = new XMLHttpRequest();
        xhr.open('post', this.props.submitUrl, true);
        xhr.onload = () => {
            this.loadMovementsFromServer(movement)
        };
        xhr.send(data);

    }

    componentDidMount() {
        this.loadMovementsFromServer();
    }
    render() {
        return (
            <div className="movementBox">
                <h1>Movements</h1>
                <SearchMovementForm onMovementsSearch={this.loadMovementsFromServer} />
                <MovementForm onMovementSubmit={this.handleMovementSubmit} />
                <MovementList data={this.state.data} />
            </div>
        );
    }
}

class MovementList extends React.Component {
    render() {
        const movementNodes = this.props.data.map(movement => (
            <Movement name={movement.name} type={movement.type}>
            </Movement>
        ));
        return <div className="movementList">{movementNodes}</div>;
    }
}

class MovementForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: '', type: '' };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleTypeChange = this.handleTypeChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }

    handleTypeChange(e) {
        this.setState({ type: e.target.value });
    }

    handleSubmit(e) {
        console.log('handlesubmit');
        e.preventDefault();
        const name = this.state.name.trim();
        const type = this.state.type.trim();
        if (!name || !type) {
            return;
        }
        this.props.onMovementSubmit({ name: name, type: type });
        this.setState({ name: '', type: '' });
    }

    render() {
        return (
            <form className="movementForm" onSubmit={this.handleSubmit}>
                <h2>Create</h2>

                <input
                    type="text"
                    placeholder="Name"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                />
                <input
                    type="text"
                    placeholder="Type"
                    value={this.state.type}
                    onChange={this.handleTypeChange}
                />
                <input type="submit" value="Post" />
            </form>
        );
    }
}

class SearchMovementForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: ''};
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleSearch = this.handleSearch.bind(this);

    }

    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
  
    handleSearch(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        if (!name) {
            return;
        }
        this.props.onMovementsSearch({ name: name});
        this.setState({ name: name});
    }

    render() {
        return (
            <form className="movementForm" onSubmit={this.handleSearch}>
                <h2>Search</h2>
                <input
                    type="text"
                    placeholder="Search by name"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                />
           
                <input type="submit" value="Post" />
            </form>
        );
    }
}


function createRemarkable() {
    var remarkable =
        'undefined' != typeof global && global.Remarkable
            ? global.Remarkable
            : window.Remarkable;

    return new remarkable();
}

class Movement extends React.Component {

    rawMarkup() {
        const md = new Remarkable();
        const rawMarkup = md.render(this.props.children.toString());
        return { __html: rawMarkup };
    }

    render() {
        const md = createRemarkable();
        return (
            <div className="movement">
                <h2 className="movementName">{this.props.name}</h2>
                <p className="movementType">{this.props.type}</p>
                {this.props.children}
            </div>
        );
    }
}

ReactDOM.render(
    <MovementBox
        url="/movements"
        searchUrl="/movements/search"
        submitUrl="/movements/new"
    //    pollInterval={2000}
    />,
    document.getElementById('content'),
);